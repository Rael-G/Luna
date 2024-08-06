using System.Numerics;
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

    public TextureTarget TextureTarget { get; }

    public PixelFormat PixelFormat { get; }

    public InternalFormat InternalFormat { get; }

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

    private Texture(string path, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, bool flipV, PixelFormat pixelFormat, InternalFormat internalFormal, TextureTarget textureTarget, string hash)
    {
        Path = path;
        _textureFilter = textureFilter;
        _textureWrap = textureWrap;
        MipmapLevel = mipmaps;
        FlipV = flipV;
        PixelFormat = pixelFormat;
        InternalFormat = internalFormal;
        TextureTarget = textureTarget;
        LoadTexture();
        _hash = string.IsNullOrEmpty(hash)? GetHashCode().ToString() : hash;
        GlErrorUtils.CheckError("Texture");
    }

    public static Texture Load(string path, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, bool flipV, PixelFormat pixelFormat, InternalFormat internalFormal, TextureTarget textureTarget, string hash = "")
    {
        var texture = TextureManager.GetTexture(hash);
        TextureManager.StartUsing(hash);
        if (texture is not null)
            return texture;
        
        texture = new(path, textureFilter, textureWrap, mipmaps, flipV, pixelFormat, internalFormal, textureTarget, hash);
        TextureManager.Cache(hash, texture);
        return texture;
    }

    public static Texture Load(Texture2D texture2D)
        => Load(texture2D.Path, texture2D.TextureFilter, texture2D.TextureWrap, 
            texture2D.MipmapLevel, texture2D.FlipV, PixelFormat.Rgba, InternalFormat.Rgba, TextureTarget.Texture2D, texture2D.GetHashCode().ToString());

    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(unit);
        _gl.BindTexture(TextureTarget, Handle);
        GlErrorUtils.CheckError("Texture Bind");
    }

    private void LoadTexture()
    {
        using var stream = File.OpenRead(Path);
        using var memoryStream = new MemoryStream();

        stream.CopyTo(memoryStream);
        Stbi.SetFlipVerticallyOnLoad(FlipV);
        using var image = Stbi.LoadFromMemory(memoryStream, 0);

        Size = new Vector2(image.Width, image.Height);

        Handle = _gl.GenTexture();
        _gl.BindTexture(TextureTarget, Handle);

        _gl.TexImage2D(TextureTarget, MipmapLevel, InternalFormat, (uint)image.Width,
            (uint)image.Height, 0, PixelFormat, PixelType.UnsignedByte, image.Data);

        TextureFilter = _textureFilter;
        TextureWrap = _textureWrap;
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
        return (Path + MipmapLevel + TextureFilter + TextureWrap + TextureTarget + PixelFormat + InternalFormat).GetHashCode();
    }
}
