using System.Numerics;
using Luna.OpenGL.Enums;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Luna.OpenGL;

public abstract class TextureBase : Disposable
{
    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    public uint Handle { get; private set; }

    public int MipmapLevel { get; }

    public ImageType ImageType { get; protected set; }

    public TextureTarget TextureTarget { get; protected set; }

    public TextureFilter TextureFilter
    {
        get => _textureFilter;
        set
        {
            _textureFilter = value;
            if (_textureFilter is TextureFilter.Bilinear)
            {
                _gl.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                _gl.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            else if (_textureFilter is TextureFilter.Trilinear)
            {
                _gl.GenerateMipmap(TextureTarget.Texture2D);
                _gl.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                _gl.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            }
            else if (_textureFilter is TextureFilter.Nearest)
            {
                _gl.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                _gl.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            }
        }
    }

    public virtual TextureWrap TextureWrap
    {
        get => _textureWrap;
        set
        {
            _textureWrap = value;
            _gl.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)_textureWrap);
            _gl.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)_textureWrap);
        }
    }

    private TextureFilter _textureFilter;
    private TextureWrap _textureWrap;

    protected string Hash { get; set; }

    protected TextureBase(TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, TextureTarget textureTarget, string hash, ImageType imageType)
    {
        _textureFilter = textureFilter;
        _textureWrap = textureWrap;
        MipmapLevel = mipmaps;
        ImageType = imageType;
        TextureTarget = textureTarget;
        Hash = hash;
    }

    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(unit);
        _gl.BindTexture(TextureTarget, Handle);
        GlErrorUtils.CheckError("Texture Bind");
    }
    public void Unbind(TextureUnit unit = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(unit);
        _gl.BindTexture(TextureTarget, 0);
        GlErrorUtils.CheckError("Texture Unbind");
    }

    protected void CreateTexture(uint width, uint height, ReadOnlySpan<byte> data, PixelFormat pixelFormat, InternalFormat internalFormat)
    {
        Handle = _gl.GenTexture();
        _gl.BindTexture(TextureTarget, Handle);

        TextureFilter = _textureFilter;
        TextureWrap = _textureWrap;

        _gl.PixelStore(GLEnum.UnpackAlignment, 1);
        _gl.TexImage2D(TextureTarget, MipmapLevel, internalFormat, width,
            height, 0, pixelFormat, PixelType.UnsignedByte, data);
        _gl.GenerateMipmap(TextureTarget);
        GlErrorUtils.CheckError("Texture CreateTexture");
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (TextureManager.StopUsing(Hash) <= 0)
        {
            TextureManager.Delete(Hash);
            _gl.DeleteTexture(Handle);
        }

        base.Dispose(disposing);
    }

    public override int GetHashCode()
    {
        return (MipmapLevel.ToString() + TextureFilter + TextureWrap + TextureTarget + ImageType).GetHashCode();
    }

    protected static (PixelFormat, InternalFormat) GetFormat(ImageType type, int numChannels = 3)
    {
        PixelFormat pixelFormat;
        InternalFormat internalFormat;

        switch (type)
        {
            case ImageType.HDR:
                pixelFormat = PixelFormat.Rgba;
                internalFormat = InternalFormat.Rgba16f; // Use a higher precision format for HDR
                break;

            case ImageType.DepthMap:
                pixelFormat = PixelFormat.DepthComponent;
                internalFormat = InternalFormat.DepthComponent; // 24-bit depth map
                break;
            
            case ImageType.StencilMap:
                pixelFormat = PixelFormat.StencilIndex;
                internalFormat = InternalFormat.StencilIndex;
                break;

            case ImageType.DepthStencilMap:
                pixelFormat = PixelFormat.DepthStencil;
                internalFormat = InternalFormat.Depth24Stencil8;
                break;

            case ImageType.Cubemap:
                pixelFormat = PixelFormat.Rgba;
                internalFormat = InternalFormat.Rgba8;
                break;

            default:
                pixelFormat = numChannels switch
                {
                    1 => PixelFormat.Red,        // Monochrome
                    2 => PixelFormat.RG,         // Two-channel (e.g., RG format)
                    3 => PixelFormat.Rgb,        // RGB
                    4 => PixelFormat.Rgba,       // RGBA
                    _ => throw new ArgumentException("Unsupported number of channels")
                };

                internalFormat = numChannels switch
                {
                    1 => InternalFormat.R8,      // 8-bit Red channel
                    2 => InternalFormat.RG8,     // 8-bit RG
                    3 => InternalFormat.Rgb8,    // 8-bit RGB
                    4 => InternalFormat.Rgba8,   // 8-bit RGBA
                    _ => throw new ArgumentException("Unsupported number of channels")
                };
                break;
        }
        
        return (pixelFormat, internalFormat);
    }
}
