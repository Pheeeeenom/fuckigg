using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using ImageMagick;
using System.Drawing.Imaging;

namespace UnityTexTool.UnityEngine
{
    public class astcencWrapper
    {
        private const string encoder = "astcenc.exe";
        private const int MAGIC_FILE_CONSTANT = 0x5CA1AB13;
        public static void EncodeASTC(byte[] inputBytes,int width,int height,int block_xsize,int block_ysize, out byte[] dstBytes)
        {
            dstBytes = null;
            string tastcpath = Path.Combine(Environment.CurrentDirectory, "temp.astc");
            string ttgapath = Path.Combine(Environment.CurrentDirectory, "temp.png");
            if (File.Exists(tastcpath)) File.Delete(tastcpath);
            if (File.Exists(ttgapath)) File.Delete(ttgapath);
            MagickReadSettings settings = new MagickReadSettings();
            settings.Format = MagickFormat.Rgba;
            settings.Width = width;
            settings.Height = height;
            ImageMagick.MagickImage im = new MagickImage(inputBytes, settings);
            im.Flip();//unity纹理是颠倒放置，要flip
            im.ToBitmap().Save(ttgapath);
            if (File.Exists(ttgapath))
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.StartInfo.FileName = encoder;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Arguments = string.Format(@"-c ""{0}"" ""{1}"" {2}x{3} -medium", ttgapath, tastcpath, block_xsize, block_ysize);
                Console.WriteLine(string.Format(@"-c ""{0}"" ""{1}"" {2}x{3} -medium", ttgapath, tastcpath, block_xsize, block_ysize));
                process.Start();
                process.WaitForExit();
            }

            if (File.Exists(tastcpath))
            {
                using (FileStream fs = File.Open(tastcpath, FileMode.Open))
                {
                    dstBytes = new byte[(int)fs.Length - 0x10];
                    fs.Seek(0x10, SeekOrigin.Begin);
                    fs.Read(dstBytes, 0, (int)fs.Length - 0x10);
                }
            }
            if (File.Exists(tastcpath)) File.Delete(tastcpath);
            if (File.Exists(ttgapath)) File.Delete(ttgapath);
        }

        public static void DecodeASTC(byte[] inputBytes, int width, int height, int block_xsize, int block_ysize, out byte[] dstBytes)
        {
            string tastcpath = Path.Combine(Environment.CurrentDirectory, "temp.astc");
            string ttgapath = Path.Combine(Environment.CurrentDirectory, "temp.tga");
            if (File.Exists(tastcpath)) File.Delete(tastcpath);
            if (File.Exists(ttgapath)) File.Delete(ttgapath);
            dstBytes = null;
            GenerateASTCFile(inputBytes, width, height, block_xsize, block_ysize);
            if (File.Exists(tastcpath))
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.StartInfo.FileName = encoder;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Arguments = string.Format(@"-d ""{0}"" ""{1}""", tastcpath, ttgapath);
                Console.WriteLine(string.Format(@"-d ""{0}"" ""{1}""", tastcpath, ttgapath));
                process.Start();
                process.WaitForExit();
            }
            if (File.Exists(ttgapath))
            {
                Console.WriteLine("load temp png");
                ImageMagick.MagickImage im = new MagickImage(ttgapath);
                im.Flip();
                dstBytes = im.GetPixels().ToByteArray(0, 0, im.Width, im.Height, "RGBA");
                im.Dispose();

            }
            else
            {
                Console.WriteLine("ERR: astcenc.exe encoding error");
            }
            if (File.Exists(tastcpath)) File.Delete(tastcpath);
            if (File.Exists(ttgapath)) File.Delete(ttgapath);
        }

        private static void GenerateASTCFile(byte[] inputBytes, int width, int height, int block_xsize, int block_ysize)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(MAGIC_FILE_CONSTANT);
                    bw.Write(block_xsize + block_ysize * 0x100 + 1 * 0x10000);
                    bw.Seek(-1, SeekOrigin.Current);
                    bw.Write(width);
                    bw.Seek(-1, SeekOrigin.Current);
                    bw.Write(height);
                    bw.Seek(-1, SeekOrigin.Current);
                    bw.Write(1);
                    bw.Seek(-1, SeekOrigin.Current);
                    bw.Write(inputBytes);
                }
                var dst = ms.ToArray();
                File.WriteAllBytes("temp.astc", dst);
            }
        }
    }
}
