
using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class Texture2DGL : Texture
{    
    public Texture2DGL(GL gl, string path, bool flipVertically, ImageType imageType, FilterMode filterMode, 
        WrapMode wrapMode, int mipmapLevel = 0) 
        : base(gl, mipmapLevel, TextureTarget.Texture2D)
    {
        using var image = LoadImageFromFile(path, flipVertically);
    
        Bind();
        GenerateTexture2D(imageType, image.NumChannels, new Vector2(image.Width, image.Height), image.Data);
        SetFilterMode(filterMode);
        SetWrapMode(wrapMode);
        Unbind();
    }

    public Texture2DGL(GL gl, Vector2 size, ImageType imageType, FilterMode filterMode, WrapMode wrapMode, 
        int mipmapLevel = 0, int samples = 0) 
        : base(gl, mipmapLevel, samples == 0 ? TextureTarget.Texture2D : TextureTarget.Texture2DMultisample)
    {
        Bind();
        GenerateTexture2D(imageType, 3, size, new ReadOnlySpan<byte>(), samples);
        if (samples == 0)
        {
            SetFilterMode(filterMode);
            SetWrapMode(wrapMode);
        }
        Unbind();
    }

    private void GenerateTexture2D(ImageType imageType, int channels, Vector2 size, ReadOnlySpan<byte> data,
        int samples = 0)
    {
        var (pixelFormat, internalFormat, pixelType) = GetFormat(imageType, channels);

        Gl.PixelStore(GLEnum.UnpackAlignment, 1);

        if (samples == 0)
        {
            Gl.TexImage2D(TextureTarget, MipmapLevel, internalFormat, (uint)size.X, (uint)size.Y, 0, 
                pixelFormat, pixelType, data);
        }
        else
        {
            Gl.TexImage2DMultisample(TextureTarget, (uint)samples, internalFormat, (uint)size.X, (uint)size.Y, true);
        }

        GlErrorUtils.CheckError("Texture2DGL GenerateTexture2D");
    }
}
