using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ImageMagick;
using UnityTexTool.UnityEngine;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.exe";
            DialogResult result = openFileDialog.ShowDialog();
            byte[] zero =  {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

            if (result == DialogResult.OK)
            {

                string unityDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                string[] unityCheck = Directory.GetFiles(unityDirectory, "UnityPlayer.dll");
                string[] garbrage = Directory.GetFiles(unityDirectory);
                foreach (string file in garbrage)
                {
                    if (file.Contains("IGG"))
                    {
                        File.Delete(file);
                    }
                }

                if (unityCheck[0] != null)
                {
                    string[] gamesList = new string[] { "PMS_Build", "Tube Tycoon" };
                    string[] unityAssets = Directory.GetFiles(unityDirectory, "sharedassets0.assets", SearchOption.AllDirectories);
                    BinaryWriter ubw = new BinaryWriter(File.Open(unityAssets[0], FileMode.Open));
                    foreach (string game in gamesList)
                    {
                        switch (game)
                        {
                            case "PMS_Build":
                                if (!openFileDialog.FileName.Contains(game)) break;
                                ubw.Seek(0x17BC0, SeekOrigin.Begin);
                                ubw.Write(zero);
                                break;
                            case "Tube Tycoon":
                                if (!openFileDialog.FileName.Contains(game)) break;
                                byte[] b = { 0x00, 0x00 };
                                ubw.Seek(0x21A0 + 8, SeekOrigin.Begin);
                                ubw.Write(b);
                                break;
                        }
                    }
                    ubw.Dispose();
                }

                byte[] magic = { 0x49, 0x47, 0x47, 0x2D };
                byte[] temp = new byte[4];
                

                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate);
                BinaryWriter bw = new BinaryWriter(fs);
                bool a;
                while (fs.Read(temp, 0, temp.Length) > 0)
                {
                    a = temp.SequenceEqual(magic);
                    if (a)
                    {
                        fs.Seek(fs.Position - 4, SeekOrigin.Begin);

                        bw.Write(zero);
                        break;
                    }

                }
                bw.Flush();
                fs.Flush();
                bw.Close();
                fs.Close();
            }
            MessageBox.Show("Done");

        }
        private static void PNG2TEX(string input, string output, string resSPath = "./")
        {
            byte[] dstTex2D = File.ReadAllBytes(output);
            Texture2D texture;
            try
            {
                texture = new Texture2D(dstTex2D);
                if (!texture.isTexture2D)
                {
                        return;
                }
                    
                
            if (texture.format == TextureFormat.Alpha8   ||
                texture.format == TextureFormat.ARGB4444 ||
                texture.format == TextureFormat.RGBA4444 ||
                texture.format == TextureFormat.RGBA32   ||
                texture.format == TextureFormat.ARGB32   ||
                texture.format == TextureFormat.RGB24    ||
                texture.format == TextureFormat.RGB565   ||
                texture.format == TextureFormat.ETC_RGB4 ||
                texture.format == TextureFormat.ETC2_RGB ||
                texture.format == TextureFormat.ETC2_RGBA8 ||
                texture.format == TextureFormat.DXT5     ||
                texture.format == TextureFormat.DXT1     ||
                texture.format == TextureFormat.ASTC_RGBA_4x4 ||
                texture.format == TextureFormat.ASTC_RGB_4x4)
                {
                    ImageMagick.MagickImage im = new MagickImage(input);

                    if ((im.Width != texture.width) || im.Height != texture.height)
                    {
                        //display error to user
                        Console.WriteLine("Error: texture is {0} x {1} , but png bitmap is {2} x {3}.Exit.",
                                            texture.width, 
                                            texture.height,
                                            im.Width, 
                                            im.Height);
                        return;
                    }

                    im.Flip();

                    byte[] sourceData = im.GetPixels().ToByteArray(0, 0, im.Width, im.Height, "RGBA");
                    byte[] outputData;
                    Console.WriteLine("Reading:{0}\n Width:{1}\n Height:{2}\n Format:{3}\n", 
                                        input, 
                                        im.Width,  
                                        im.Height, 
                                        texture.format.ToString());

                    Console.WriteLine("Converting...");

                    TextureConverter.CompressTexture(texture.format, im.Width, im.Height, sourceData, out outputData);

                    if (texture.bMipmap && texture.mipmapCount >= 3)
                    {
                        Console.WriteLine("Generating Mipmap...");
                        for (var m = 0; m <= 3; m++)
                        {

                            im.AdaptiveResize(im.Width / 2, im.Height / 2);
                            Console.WriteLine("Generating ...{0}x{1}", 
                                              im.Width, 
                                              im.Height);

                            sourceData = im.GetPixels().ToByteArray(0, 0, im.Width, im.Height, "RGBA");

                            byte[] dst;

                            TextureConverter.CompressTexture(texture.format, im.Width, im.Height, sourceData, out dst);

                            outputData = outputData.Concat(dst).ToArray();
                        }
                    }
                    if (outputData != null)
                    {
                        if ((outputData.Length > texture.textureSize))
                        {
                            Console.WriteLine("Error: generated data size {0}> original texture size {1}. Exit.", outputData.Length, texture.textureSize);
                        }
                        if (texture.bHasResSData == true)
                        {
                            output = string.Format("{0}/{1}", resSPath, texture.resSName);

                        }
                        if (File.Exists(output))
                        {

                            Console.WriteLine("Writing...{0}", output);
                            using (FileStream fs = File.Open(output, FileMode.Open, FileAccess.ReadWrite))
                            {
                                fs.Seek(texture.dataPos, SeekOrigin.Begin);
                                fs.Write(outputData, 0, outputData.Length);
                                fs.Flush();
                            }
                            Console.WriteLine("File Created...");
                        }

                        else
                        {
                            Console.WriteLine("Error: file {0} not found", output);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: generated data size {0}> original texture size {1}. Exit.", outputData.Length, texture.textureSize);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:Not A valid Texture: {0}", ex.ToString());
            }
        }

        private static void TEX2PNG(string input, string output, string resSPath = "")
        {
            byte[] inputArray = File.ReadAllBytes(input);
            Texture2D texture = new Texture2D(inputArray, resSPath);
            if (!texture.isTexture2D || texture.GetPixels() == null)
            {
                return;
            }

            Console.WriteLine("Reading: {0}\n Width: {1}\n Height: {2}\n Format: {3}\n Dimension: {4}\n Filter Mode: {5}\n Wrap Mode: {6}\n Mipmap: {7}",
                                input,
                                texture.width,
                                texture.height,
                                texture.format.ToString(),
                                texture.dimension.ToString(),
                                texture.filterMode.ToString(),
                                texture.wrapMode.ToString(),
                                texture.bMipmap);

            if (texture.format == TextureFormat.Alpha8 ||
                texture.format == TextureFormat.ARGB4444 ||
                texture.format == TextureFormat.RGBA4444 ||
                texture.format == TextureFormat.RGBA32 ||
                texture.format == TextureFormat.ARGB32 ||
                texture.format == TextureFormat.RGB24 ||
                texture.format == TextureFormat.RGB565 ||
                texture.format == TextureFormat.ETC_RGB4 ||
                texture.format == TextureFormat.ETC2_RGB ||
                texture.format == TextureFormat.ETC2_RGBA8 ||
                texture.format == TextureFormat.DXT5 ||
                texture.format == TextureFormat.DXT1 ||
                texture.format == TextureFormat.ASTC_RGBA_4x4 ||
                texture.format == TextureFormat.ASTC_RGB_4x4
                )
            {
                MagickReadSettings settings = new MagickReadSettings();
                settings.Format = MagickFormat.Rgba;
                settings.Width = texture.width;
                settings.Height = texture.height;

                ImageMagick.MagickImage im = new MagickImage(texture.GetPixels(), settings);
                im.Flip();
                im.ToBitmap().Save(output);
            }
        }

        private void btnTEXToPNG_Click(object sender, EventArgs e)
        {
            //add open file dialog + for each for multiple files
            TEX2PNG("test.tex",  "test.png");
        }

        private void btnPNGtoTEX_Click(object sender, EventArgs e)
        {
            //modifies the CURRENT .tex file to match the PNG
            //you NEED to keep the .tex file. DO NOT DELETE
            PNG2TEX("test.png", "test.tex");
        }

        private void btnUnpack_Click(object sender, EventArgs e)
        {
           
        }
    }
}
