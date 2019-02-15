using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVRTexLibNET;
using Nvidia.TextureTools;
using System.Runtime.InteropServices;

namespace UnityTexTool.UnityEngine
{
    /*
     * code from mono game framework
     * Microsoft Public License (Ms-PL)
     * MonoGame - Copyright © 2009-2016 The MonoGame Team
     * All rights reserved.

    */
    public class DxtCompressor:IDisposable
    {
        public bool Compress(byte[] sourceData, int width, int height, Nvidia.TextureTools.Format textureformat, out byte[] output)
        {
            output = new byte[] { };
            var dxtCompressor = new Nvidia.TextureTools.Compressor();
            var inputOptions = new Nvidia.TextureTools.InputOptions();
            if (textureformat == Format.DXT1)
            {
                inputOptions.SetAlphaMode(Nvidia.TextureTools.AlphaMode.None);
            }
            else
            {
                inputOptions.SetAlphaMode(Nvidia.TextureTools.AlphaMode.Premultiplied);
            }
            inputOptions.SetTextureLayout(TextureType.Texture2D, width, height, 1);

            for (var x = 0; x < sourceData.Length; x += 4)
            {
                sourceData[x] ^= sourceData[x + 2];
                sourceData[x + 2] ^= sourceData[x];
                sourceData[x] ^= sourceData[x + 2];
            }
            var dataHandle = GCHandle.Alloc(sourceData, GCHandleType.Pinned);
            try
            {
                var dataPtr = dataHandle.AddrOfPinnedObject();

                inputOptions.SetMipmapData(dataPtr, width, height, 1, 0, 0);
                inputOptions.SetMipmapGeneration(false);
                inputOptions.SetGamma(1.0f, 1.0f);

                var compressionOptions = new CompressionOptions();
                compressionOptions.SetFormat(textureformat);
                compressionOptions.SetQuality(Nvidia.TextureTools.Quality.Normal);

                var outputOptions = new OutputOptions();
                outputOptions.SetOutputHeader(false);
                using (var handler = new DxtDataHandler(output, outputOptions))
                {
                    dxtCompressor.Compress(inputOptions, compressionOptions, outputOptions);
                    output = handler.dst;
                }


            }
            finally
            {
                dataHandle.Free();
            }
            return true;
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed objects
                    // ...
                }

                disposed = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    
    public class DxtDataHandler : IDisposable
    {
        public byte[] dst;
        private byte[] _result;
        byte[] _buffer;
        int _offset;

        GCHandle delegateHandleBeginImage;
        GCHandle delegateHandleWriteData;

        public OutputOptions.WriteDataDelegate WriteData { get; private set; }
        public OutputOptions.ImageDelegate BeginImage { get; private set; }

        public DxtDataHandler(byte[] result, OutputOptions outputOptions)
        {

            _result = result;
            WriteData = new OutputOptions.WriteDataDelegate(WriteDataInternal);
            BeginImage = new OutputOptions.ImageDelegate(BeginImageInternal);

            // Keep the delegate from being re-located or collected by the garbage collector.
            delegateHandleBeginImage = GCHandle.Alloc(BeginImage);
            delegateHandleWriteData = GCHandle.Alloc(WriteData);

            outputOptions.SetOutputHandler(BeginImage, WriteData);
        }

        ~DxtDataHandler()
        {
            Dispose(false);
        }

        void BeginImageInternal(int size, int width, int height, int depth, int face, int miplevel)
        {
            _buffer = new byte[size];
            _offset = 0;
        }

        bool WriteDataInternal(IntPtr data, int length)
        {
            Marshal.Copy(data, _buffer, _offset, length);
            _offset += length;
            if (_offset == _buffer.Length)
                dst = (_buffer);
            return true;
        }

        #region IDisposable Support
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed objects
                    // ...
                }

                // Release native objects
                delegateHandleBeginImage.Free();
                delegateHandleWriteData.Free();

                disposed = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
