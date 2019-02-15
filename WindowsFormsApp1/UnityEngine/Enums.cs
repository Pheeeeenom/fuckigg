using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTexTool.UnityEngine
{
    public enum TextureDimension
    {
        /// <summary>
        ///   <para>Texture type is not initialized or unknown.</para>
        /// </summary>
        Unknown = -1,
        /// <summary>
        ///   <para>No texture is assigned.</para>
        /// </summary>
        None,
        /// <summary>
        ///   <para>Any texture type.</para>
        /// </summary>
        Any,
        /// <summary>
        ///   <para>2D texture (Texture2D).</para>
        /// </summary>
        Tex2D,
        /// <summary>
        ///   <para>3D volume texture (Texture3D).</para>
        /// </summary>
        Tex3D,
        /// <summary>
        ///   <para>Cubemap texture.</para>
        /// </summary>
        Cube,
        /// <summary>
        ///   <para>2D array texture (Texture2DArray).</para>
        /// </summary>
        Tex2DArray,
        /// <summary>
        ///   <para>Cubemap array texture (CubemapArray).</para>
        /// </summary>
        CubeArray
    }
    /// <summary>
	///   <para>Filtering mode for textures. Corresponds to the settings in a.</para>
	/// </summary>
    public enum FilterMode
    {
        /// <summary>
        ///   <para>Point filtering - texture pixels become blocky up close.</para>
        /// </summary>
        Point,
        /// <summary>
        ///   <para>Bilinear filtering - texture samples are averaged.</para>
        /// </summary>
        Bilinear,
        /// <summary>
        ///   <para>Trilinear filtering - texture samples are averaged and also blended between mipmap levels.</para>
        /// </summary>
        Trilinear
    }
    /// <summary>
	///   <para>Wrap mode for textures.</para>
	/// </summary>
	public enum TextureWrapMode
    {
        /// <summary>
        ///   <para>Tiles the texture, creating a repeating pattern.</para>
        /// </summary>
        Repeat,
        /// <summary>
        ///   <para>Clamps the texture to the last pixel at the border.</para>
        /// </summary>
        Clamp
    }
    /// <summary>
	///   <para>Format used when creating textures from scripts.</para>
	/// </summary>
	public enum TextureFormat
    {
        /// <summary>
        ///   <para>Alpha-only texture format.</para>
        /// </summary>
        Alpha8 = 1,
        /// <summary>
        ///   <para>A 16 bits/pixel texture format. Texture stores color with an alpha channel.</para>
        /// </summary>
        ARGB4444,
        /// <summary>
        ///   <para>Color texture format, 8-bits per channel.</para>
        /// </summary>
        RGB24,
        /// <summary>
        ///   <para>Color with alpha texture format, 8-bits per channel.</para>
        /// </summary>
        RGBA32,
        /// <summary>
        ///   <para>Color with alpha texture format, 8-bits per channel.</para>
        /// </summary>
        ARGB32,
        /// <summary>
        ///   <para>A 16 bit color texture format.</para>
        /// </summary>
        RGB565 = 7,
        /// <summary>
        ///   <para>A 16 bit color texture format that only has a red channel.</para>
        /// </summary>
        R16 = 9,
        /// <summary>
        ///   <para>Compressed color texture format.</para>
        /// </summary>
        DXT1,
        /// <summary>
        ///   <para>Compressed color with alpha channel texture format.</para>
        /// </summary>
        DXT5 = 12,
        /// <summary>
        ///   <para>Color and alpha  texture format, 4 bit per channel.</para>
        /// </summary>
        RGBA4444,
        /// <summary>
        ///   <para>Color with alpha texture format, 8-bits per channel.</para>
        /// </summary>
        BGRA32,
        /// <summary>
        ///   <para>Scalar (R)  texture format, 16 bit floating point.</para>
        /// </summary>
        RHalf,
        /// <summary>
        ///   <para>Two color (RG)  texture format, 16 bit floating point per channel.</para>
        /// </summary>
        RGHalf,
        /// <summary>
        ///   <para>RGB color and alpha texture format, 16 bit floating point per channel.</para>
        /// </summary>
        RGBAHalf,
        /// <summary>
        ///   <para>Scalar (R) texture format, 32 bit floating point.</para>
        /// </summary>
        RFloat,
        /// <summary>
        ///   <para>Two color (RG)  texture format, 32 bit floating point per channel.</para>
        /// </summary>
        RGFloat,
        /// <summary>
        ///   <para>RGB color and alpha texture format,  32-bit floats per channel.</para>
        /// </summary>
        RGBAFloat,
        /// <summary>
        ///   <para>A format that uses the YUV color space and is often used for video encoding or playback.</para>
        /// </summary>
        YUY2,
        /// <summary>
        ///   <para>Compressed one channel (R) texture format.</para>
        /// </summary>
        BC4 = 26,
        /// <summary>
        ///   <para>Compressed two-channel (RG) texture format.</para>
        /// </summary>
        BC5,
        /// <summary>
        ///   <para>HDR compressed color texture format.</para>
        /// </summary>
        BC6H = 24,
        /// <summary>
        ///   <para>High quality compressed color texture format.</para>
        /// </summary>
        BC7,
        /// <summary>
        ///   <para>Compressed color texture format with Crunch compression for small storage sizes.</para>
        /// </summary>
        DXT1Crunched = 28,
        /// <summary>
        ///   <para>Compressed color with alpha channel texture format with Crunch compression for small storage sizes.</para>
        /// </summary>
        DXT5Crunched,
        /// <summary>
        ///   <para>PowerVR (iOS) 2 bits/pixel compressed color texture format.</para>
        /// </summary>
        PVRTC_RGB2,
        /// <summary>
        ///   <para>PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format.</para>
        /// </summary>
        PVRTC_RGBA2,
        /// <summary>
        ///   <para>PowerVR (iOS) 4 bits/pixel compressed color texture format.</para>
        /// </summary>
        PVRTC_RGB4,
        /// <summary>
        ///   <para>PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format.</para>
        /// </summary>
        PVRTC_RGBA4,
        /// <summary>
        ///   <para>ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.</para>
        /// </summary>
        ETC_RGB4,
        /// <summary>
        ///   <para>ATC (ATITC) 4 bits/pixel compressed RGB texture format.</para>
        /// </summary>
        ATC_RGB4,
        /// <summary>
        ///   <para>ATC (ATITC) 8 bits/pixel compressed RGB texture format.</para>
        /// </summary>
        ATC_RGBA8,
        /// <summary>
        ///   <para>ETC2  EAC (GL ES 3.0) 4 bitspixel compressed unsigned single-channel texture format.</para>
        /// </summary>
        EAC_R = 41,
        /// <summary>
        ///   <para>ETC2  EAC (GL ES 3.0) 4 bitspixel compressed signed single-channel texture format.</para>
        /// </summary>
        EAC_R_SIGNED,
        /// <summary>
        ///   <para>ETC2  EAC (GL ES 3.0) 8 bitspixel compressed unsigned dual-channel (RG) texture format.</para>
        /// </summary>
        EAC_RG,
        /// <summary>
        ///   <para>ETC2  EAC (GL ES 3.0) 8 bitspixel compressed signed dual-channel (RG) texture format.</para>
        /// </summary>
        EAC_RG_SIGNED,
        /// <summary>
        ///   <para>ETC2 (GL ES 3.0) 4 bits/pixel compressed RGB texture format.</para>
        /// </summary>
        ETC2_RGB,
        /// <summary>
        ///   <para>ETC2 (GL ES 3.0) 4 bits/pixel RGB+1-bit alpha texture format.</para>
        /// </summary>
        ETC2_RGBA1,
        /// <summary>
        ///   <para>ETC2 (GL ES 3.0) 8 bits/pixel compressed RGBA texture format.</para>
        /// </summary>
        ETC2_RGBA8,
        /// <summary>
        ///   <para>ASTC (4x4 pixel block in 128 bits) compressed RGB texture format.</para>
        /// </summary>
        ASTC_RGB_4x4,
        /// <summary>
        ///   <para>ASTC (5x5 pixel block in 128 bits) compressed RGB texture format.</para>
        /// </summary>
        ASTC_RGB_5x5,
        /// <summary>
        ///   <para>ASTC (6x6 pixel block in 128 bits) compressed RGB texture format.</para>
        /// </summary>
        ASTC_RGB_6x6,
        /// <summary>
        ///   <para>ASTC (8x8 pixel block in 128 bits) compressed RGB texture format.</para>
        /// </summary>
        ASTC_RGB_8x8,
        /// <summary>
        ///   <para>ASTC (10x10 pixel block in 128 bits) compressed RGB texture format.</para>
        /// </summary>
        ASTC_RGB_10x10,
        /// <summary>
        ///   <para>ASTC (12x12 pixel block in 128 bits) compressed RGB texture format.</para>
        /// </summary>
        ASTC_RGB_12x12,
        /// <summary>
        ///   <para>ASTC (4x4 pixel block in 128 bits) compressed RGBA texture format.</para>
        /// </summary>
        ASTC_RGBA_4x4,
        /// <summary>
        ///   <para>ASTC (5x5 pixel block in 128 bits) compressed RGBA texture format.</para>
        /// </summary>
        ASTC_RGBA_5x5,
        /// <summary>
        ///   <para>ASTC (6x6 pixel block in 128 bits) compressed RGBA texture format.</para>
        /// </summary>
        ASTC_RGBA_6x6,
        /// <summary>
        ///   <para>ASTC (8x8 pixel block in 128 bits) compressed RGBA texture format.</para>
        /// </summary>
        ASTC_RGBA_8x8,
        /// <summary>
        ///   <para>ASTC (10x10 pixel block in 128 bits) compressed RGBA texture format.</para>
        /// </summary>
        ASTC_RGBA_10x10,
        /// <summary>
        ///   <para>ASTC (12x12 pixel block in 128 bits) compressed RGBA texture format.</para>
        /// </summary>
        ASTC_RGBA_12x12,
        /// <summary>
        ///   <para>ETC 4 bits/pixel compressed RGB texture format.</para>
        /// </summary>
        ETC_RGB4_3DS,
        /// <summary>
        ///   <para>ETC 4 bitspixel RGB + 4 bitspixel Alpha compressed texture format.</para>
        /// </summary>
        ETC_RGBA8_3DS,



    }
    

}
