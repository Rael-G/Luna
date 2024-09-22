using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public abstract class TextureBase : Disposable
{
    protected static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    public uint Handle { get; protected set; }

    public int MipmapLevel { get; }

    public ImageType ImageType { get; protected set; }

    public TextureTarget TextureTarget { get; protected set; }

    public virtual TextureFilter TextureFilter
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

    protected TextureFilter _textureFilter;
    protected TextureWrap _textureWrap;

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

    public override void Dispose(bool disposing)
    {
        if (TextureManager.StopUsing(Hash) <= 0)
        {
            if (_disposed) return;

            TextureManager.Delete(Hash);
            _gl.DeleteTexture(Handle);

            base.Dispose(disposing);
        }

    }

    protected static (PixelFormat, InternalFormat) GetFormat(ImageType type, int numChannels = 3)
    {
        var pixelFormat = numChannels switch
        {
            1 => PixelFormat.Red, 
            2 => PixelFormat.RG,
            3 => PixelFormat.Rgb,
            4 => PixelFormat.Rgba,
            _ => throw new ArgumentException("Unsupported number of channels")
        };

        var internalFormat = type switch
        {
            ImageType.Linear => numChannels switch
            {
                1 => InternalFormat.R8,
                2 => InternalFormat.RG8,
                3 => InternalFormat.Rgb8,
                4 => InternalFormat.Rgba8,
                _ => throw new ArgumentException("Unsupported number of channels")
            },
            _ => numChannels switch
            {
                1 => InternalFormat.R8,
                2 => InternalFormat.RG8,
                3 => InternalFormat.Srgb8,
                4 => InternalFormat.Srgb8Alpha8,
                _ => throw new ArgumentException("Unsupported number of channels")
            },
        };

        return (pixelFormat, internalFormat);
    }
}
