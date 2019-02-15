using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core.IO;


namespace UnityTexTool.UnityEngine
{
    public class Texture2D
    {
        public string name;
        public bool isTexture2D = true;
        public int masterTextureLimit { get; set; }
        public int anisotropicFiltering { get; set; }
        public int width;
        public int height;
        public int textureSize;
        public int dataSize;
        public TextureDimension dimension;
        public FilterMode filterMode;
        public int anisoLevel;
        public TextureWrapMode wrapMode;
        public float mipMapBias;
        public bool bMipmap = false;
        public int mipmapCount;
        public int texelSizeX;
        public int texelSizeY;
        public TextureFormat format;
        public bool bHasResSData = false;
        public long dataPos;
        public string resSName = "";
        byte[] textureData;

        public Texture2D(byte[] input, string resSFilePath = "")
        {
            this.LoadImage(input);
            /*
            try
            {
                this.LoadImage(input);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got error:{0}",e.Message);
            }*/
        }
        public void LoadImage(byte[] data, string resSFilePath = "")
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (EndianBinaryReader br = new EndianBinaryReader(ms , Endian.LittleEndian))
                {
                    int name_length = br.ReadInt32();
                    this.name = Encoding.UTF8.GetString(br.ReadBytes(name_length));
                    var pos = br.BaseStream.Position;
                    if (pos % 4 != 0)
                    {
                        br.BaseStream.Seek(4 - pos % 4, SeekOrigin.Current);
                    }
                    this.width = br.ReadInt32();
                    this.height = br.ReadInt32();
                    if ((this.width > 4096) || (this.height > 4096))
                    {
                        isTexture2D = false;
                        Console.WriteLine("Got error:{0}", "Width/Height Error\n Not a unity Texture2D data");
                        return;
                    }
                    this.textureSize = br.ReadInt32();
                    if ((this.width == 0) || (this.height == 0))
                    {
                        isTexture2D = false;
                        Console.WriteLine("Got error:{0}", "Width/Height Error\n Empty Texture");
                        return;
                    }
                    int tmp_pos = (int)br.BaseStream.Position;
                    
                    this.format = (TextureFormat)br.ReadInt32();
                    this.mipmapCount = br.ReadInt32();
                    if (this.mipmapCount > 1)
                    {
                        this.bMipmap = true;
                    }
                    int readable = br.ReadInt32();
                    int imageCount = br.ReadInt32();
                    this.dimension = (TextureDimension)br.ReadInt32();
                    this.filterMode = (FilterMode)br.ReadInt32();
                    this.anisoLevel = br.ReadInt32();
                    this.mipMapBias = br.ReadInt32();
                    this.wrapMode = (TextureWrapMode)br.ReadInt32();

                    int lightmapFormat = br.ReadInt32();
                    int colorSpace = br.ReadInt32();

                    this.dataSize = br.ReadInt32();
                    if (this.dataSize <= 0)
                    {
                        Console.WriteLine("Got error:{0}", "Data Length Error\n Not a unity Texture2D data");
                        isTexture2D = false;
                        return;
                    }
                    if (this.dataSize == 0)
                    {
                        
                        this.dataPos = br.ReadInt32();
                        this.dataSize = br.ReadInt32();
                        int name_len = br.ReadInt32();

                        if (this.dataSize <= 0)
                        {
                            Console.WriteLine("Got error:{0}", "Data Length is 0\n Not a unity Texture2D data");
                            isTexture2D = false;
                            return;
                        }

                        this.resSName = Encoding.UTF8.GetString(br.ReadBytes(name_len));
                        if (resSFilePath == "") resSFilePath = "./";
                        string rname = string.Format("{0}\\{1}", resSFilePath, this.resSName);
                        if (File.Exists(rname))
                        {
                            this.bHasResSData = true; // 图像数据在resS中
                            
                            FileStream fs = File.Open(rname, FileMode.Open, FileAccess.Read);
                            EndianBinaryReader bs = new EndianBinaryReader(fs);
                            bs.BaseStream.Seek(this.dataPos, SeekOrigin.Begin);
                            textureData = bs.ReadBytes(this.dataSize);
                            bs.Close();
                            fs.Close();
                        }
                        else
                        {
                            Console.WriteLine("Got error:{0}", "resS File not found\n can't get Texture2D data");
                            isTexture2D = false;
                            return;
                        }
                    }
                    else
                    {
                        this.resSName = this.name;
                        this.dataPos = br.BaseStream.Position;
                        textureData = br.ReadBytes(this.dataSize);

                    }
                    

                }
            }
        }

        public byte[] GetPixels()
        {
            byte[] result;
            TextureConverter.DecompressTexture(this.format, this.width, this.height, textureData,out result);
            return result;
        }
    }
}
