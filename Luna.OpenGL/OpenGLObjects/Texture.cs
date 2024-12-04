using System.Numerics;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Luna.OpenGL;

public abstract class Texture : Disposable
{
    public uint Handle { get; private set; }

    public TextureTarget TextureTarget { get; }

    public int MipmapLevel { get; }

    protected readonly GL Gl;

    public Texture(GL gl, int mipmapLevel, TextureTarget textureTarget)
    {
        Gl = gl;
        Handle = Gl.GenTexture();
        TextureTarget = textureTarget;
        MipmapLevel = mipmapLevel;
    }

    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        Gl.ActiveTexture(unit);
        Gl.BindTexture(TextureTarget, Handle);

        GlErrorUtils.CheckError("Texture Bind");
    }

    public void Unbind(TextureUnit unit = TextureUnit.Texture0)
    {
        Gl.ActiveTexture(unit);
        Gl.BindTexture(TextureTarget, 0);

        GlErrorUtils.CheckError("Texture Unbind");
    }

    public void SetBorderColor(Color color)
    {
        Gl.TextureParameter(Handle, TextureParameterName.TextureBorderColor, color.ToFloatArray());

        GlErrorUtils.CheckError("Texture SetBorderColor");
    }

    protected void SetFilterMode(FilterMode filterMode)
    {
        GlErrorUtils.CheckError("Texture Before SetFilterMode");

        int min = (int)TextureMinFilter.Linear;
        int mag =  (int)TextureMagFilter.Linear;

        if (filterMode is FilterMode.Nearest)
        {
            min = (int)TextureMinFilter.Nearest;
            mag = (int)TextureMagFilter.Nearest;
        }
        else if (filterMode is FilterMode.Trilinear)
        {
            Gl.GenerateMipmap(TextureTarget.Texture2D);
            min = (int)TextureMinFilter.LinearMipmapLinear;
            mag = (int)TextureMagFilter.Linear;
            GlErrorUtils.CheckError("Texture GenerateMipmap");
        }

        Gl.TextureParameter(Handle, TextureParameterName.TextureMinFilter, min);
        Gl.TextureParameter(Handle, TextureParameterName.TextureMagFilter, mag);

        GlErrorUtils.CheckError("Texture SetFilterMode");
    }

    protected virtual void SetWrapMode(WrapMode wrapMode)
    {
        GlErrorUtils.CheckError("Texture Before SetWrapMode");

        Gl.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)wrapMode);
        Gl.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)wrapMode);

        GlErrorUtils.CheckError("Texture SetWrapMode");
    }

    protected static StbiImage LoadImageFromFile(string path, bool flipVertically)
    {
        using var stream = File.OpenRead(path);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        Stbi.SetFlipVerticallyOnLoad(flipVertically);
        return Stbi.LoadFromMemory(memoryStream, 0);
    }

    protected static (PixelFormat, InternalFormat, PixelType) GetFormat(ImageType type, int numChannels = 3)
    {
        if (type == ImageType.DeathMap)
        {
            return (PixelFormat.DepthComponent, InternalFormat.DepthComponent, PixelType.Float);
        }

        var pixelFormat = numChannels switch
        {
            1 => PixelFormat.Red,
            2 => PixelFormat.RG,
            3 => PixelFormat.Rgb,
            4 => PixelFormat.Rgba,
            _ => throw new ArgumentException("Unsupported number of channels")
        };

        return type switch
        {
            ImageType.HDR => (pixelFormat, 
                            numChannels switch
                            {
                                3 => InternalFormat.Rgb16f,
                                4 => InternalFormat.Rgba16f,
                                _ => throw new ArgumentException("HDR supports only 3 or 4 channels")
                            }, 
                            PixelType.Float),

            ImageType.Linear => (pixelFormat, 
                                numChannels switch
                                {
                                    1 => InternalFormat.R8,
                                    2 => InternalFormat.RG8,
                                    3 => InternalFormat.Rgb8,
                                    4 => InternalFormat.Rgba8,
                                    _ => throw new ArgumentException("Unsupported number of channels")
                                }, 
                                PixelType.UnsignedByte),

            _ => (pixelFormat, 
                numChannels switch
                {
                    1 => InternalFormat.R8,
                    2 => InternalFormat.RG8,
                    3 => InternalFormat.Srgb8,
                    4 => InternalFormat.Srgb8Alpha8,
                    _ => throw new ArgumentException("Unsupported number of channels")
                }, 
                PixelType.UnsignedByte),
        };
    }

    public override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_disposed) return;

            Gl.DeleteTexture(Handle);

            base.Dispose(disposing);
        }
    }
}
