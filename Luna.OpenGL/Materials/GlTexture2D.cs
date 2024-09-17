using System.Numerics;
using Luna.OpenGL.Enums;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Luna.OpenGL;

public class GlTexture2D : TextureBase
{
    public string Path { get; set; }
    
    public Vector2 Size { get; private set; }

    public bool FlipV { get; }

    protected GlTexture2D(string path, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, 
        bool flipV, TextureTarget textureTarget, string hash, ImageType imageType)
        : base(textureFilter, textureWrap, mipmaps, textureTarget, hash, imageType)
    {
        Path = path;
        FlipV = flipV;
        ImageType = imageType;
        //Hash = string.IsNullOrEmpty(hash)? GetHashCode().ToString() : hash;
    }

    public static GlTexture2D Load(string path, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, bool flipV, TextureTarget textureTarget, string hash = "", ImageType imageType = ImageType.Standard)
    {
        var texture = TextureManager.GetTexture(hash) as GlTexture2D;
        TextureManager.StartUsing(hash);
        if (texture is not null)
            return texture;
        
        texture = new GlTexture2D(path, textureFilter, textureWrap, mipmaps, flipV, textureTarget, hash, imageType);
        texture.LoadTexture();
        TextureManager.Cache(hash, texture);
        GlErrorUtils.CheckError("GlTexture2D Load");
        return texture;
    }

    public static GlTexture2D Load(uint width, uint height, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, TextureTarget textureTarget, ImageType imageType = ImageType.Standard)
    {
        var texture = new GlTexture2D("", textureFilter, textureWrap, mipmaps, false, textureTarget, "", imageType);
        var (pFmt, iFmt) = GetFormat(imageType);
        texture.CreateTexture(width, height, new ReadOnlySpan<byte>(), pFmt, iFmt);
        GlErrorUtils.CheckError("GlTexture2D Load");
        return texture;
    }

    public static GlTexture2D Load(Texture2D texture2D)
        => Load(texture2D.Path, texture2D.TextureFilter, texture2D.TextureWrap, 
            texture2D.MipmapLevel, texture2D.FlipV, TextureTarget.Texture2D, texture2D.GetHashCode().ToString());

    private void LoadTexture()
    {
        using var stream = File.OpenRead(Path);
        using var memoryStream = new MemoryStream();

        stream.CopyTo(memoryStream);
        Stbi.SetFlipVerticallyOnLoad(FlipV);
        using var image = Stbi.LoadFromMemory(memoryStream, 0);
        var (pFmt, iFmt) = GetFormat(ImageType, image.NumChannels);
        Size = new Vector2(image.Width, image.Height);
        CreateTexture((uint)image.Width, (uint)image.Height, image.Data, pFmt, iFmt);
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
        GlErrorUtils.CheckError("GlTexture2D CreateTexture");
    }

    public override int GetHashCode()
    {
        return Path.GetHashCode() + base.GetHashCode();
    }
}
