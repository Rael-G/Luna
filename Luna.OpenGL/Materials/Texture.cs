using System.Numerics;
using Luna.OpenGL.Enums;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Luna.OpenGL;

public class Texture : Disposable
{
    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    public uint Handle { get; private set; }

    public string Path { get; }

    public Vector2 Size { get; private set; }

    public int MipmapLevel { get; }

    public ImageType ImageType { get; }

    public TextureTarget TextureTarget { get; }

    public bool FlipV { get; }

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

    public TextureWrap TextureWrap
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

    private string _hash;

    private Texture(string path, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, bool flipV, TextureTarget textureTarget, string hash, ImageType imageType)
    {
        Path = path;
        _textureFilter = textureFilter;
        _textureWrap = textureWrap;
        MipmapLevel = mipmaps;
        FlipV = flipV;
        ImageType = imageType;
        TextureTarget = textureTarget;
        _hash = string.IsNullOrEmpty(hash)? GetHashCode().ToString() : hash;
    }

    public static Texture Load(string path, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, bool flipV, TextureTarget textureTarget, string hash = "", ImageType imageType = ImageType.Standard)
    {
        var texture = TextureManager.GetTexture(hash);
        TextureManager.StartUsing(hash);
        if (texture is not null)
            return texture;
        
        texture = new(path, textureFilter, textureWrap, mipmaps, flipV, textureTarget, hash, imageType);
        texture.LoadTexture();
        TextureManager.Cache(hash, texture);
        GlErrorUtils.CheckError("Texture Load");
        return texture;
    }

    public static Texture Load(uint width, uint height, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, TextureTarget textureTarget, ImageType imageType = ImageType.Standard)
    {
        var texture = new Texture("", textureFilter, textureWrap, mipmaps, false, textureTarget, "", imageType);
        var (pFmt, iFmt) = texture.GetFormat();
        texture.CreateTexture(width, height, new ReadOnlySpan<byte>(), pFmt, iFmt);
        GlErrorUtils.CheckError("Texture Load");
        return texture;
    }

    public static Texture Load(Texture2D texture2D)
        => Load(texture2D.Path, texture2D.TextureFilter, texture2D.TextureWrap, 
            texture2D.MipmapLevel, texture2D.FlipV, TextureTarget.Texture2D, texture2D.GetHashCode().ToString());

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

    private void LoadTexture()
    {
        using var stream = File.OpenRead(Path);
        using var memoryStream = new MemoryStream();

        stream.CopyTo(memoryStream);
        Stbi.SetFlipVerticallyOnLoad(FlipV);
        using var image = Stbi.LoadFromMemory(memoryStream, 0);
        var (pFmt, iFmt) = GetFormat(image.NumChannels);
        CreateTexture((uint)image.Width, (uint)image.Height, image.Data, pFmt, iFmt);
    }

    private void CreateTexture(uint width, uint height, ReadOnlySpan<byte> data, PixelFormat pixelFormat, InternalFormat internalFormat)
    {
        Handle = _gl.GenTexture();
        _gl.BindTexture(TextureTarget, Handle);

        TextureFilter = _textureFilter;
        TextureWrap = _textureWrap;
        Size = new Vector2(width, height);

        _gl.PixelStore(GLEnum.UnpackAlignment, 1);
        _gl.TexImage2D(TextureTarget, MipmapLevel, internalFormat, width,
            height, 0, pixelFormat, PixelType.UnsignedByte, data);
        _gl.GenerateMipmap(TextureTarget);
        GlErrorUtils.CheckError("Texture CreateTexture");
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (TextureManager.StopUsing(_hash) <= 0)
        {
            TextureManager.Delete(_hash);
            _gl.DeleteTexture(Handle);
        }

        base.Dispose(disposing);
    }

    public override int GetHashCode()
    {
        return (Path + MipmapLevel + TextureFilter + TextureWrap + TextureTarget + ImageType).GetHashCode();
    }

    private (PixelFormat, InternalFormat) GetFormat(int numChannels = 3)
    {
        PixelFormat pixelFormat;
        InternalFormat internalFormat;

        switch (ImageType)
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
