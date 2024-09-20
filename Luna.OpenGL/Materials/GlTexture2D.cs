using System.Numerics;
using Luna.OpenGL.Enums;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Luna.OpenGL;

public class GlTexture2D : TextureBase
{
    public string Path { get; }
    public Vector2 Size { get; private set; }
    public bool FlipVertically { get; }

    protected GlTexture2D(string path, TextureFilter filter, TextureWrap wrap, int mipmaps, 
        bool flipVertically, TextureTarget target, string hash, ImageType imageType)
        : base(filter, wrap, mipmaps, target, hash, imageType)
    {
        Path = path;
        FlipVertically = flipVertically;
    }

    public static GlTexture2D Create(Texture2D texture2D)
    {
        return string.IsNullOrEmpty(texture2D.Path)
            ? CreateEmptyTexture(texture2D)
            : CreateFromFile(texture2D);
    }

    private static GlTexture2D CreateFromFile(Texture2D texture2D)
    {
        var texture = new GlTexture2D(texture2D.Path, texture2D.TextureFilter, texture2D.TextureWrap, texture2D.MipmapLevel, 
            texture2D.FlipV, TextureTarget.Texture2D, texture2D.Hash, ImageType.Standard);

        texture.LoadTextureFromFile();
        return texture;
    }

    private static GlTexture2D CreateEmptyTexture(Texture2D texture2D)
    {
        return new GlTexture2D(string.Empty, texture2D.TextureFilter, texture2D.TextureWrap, texture2D.MipmapLevel, 
            false, TextureTarget.Texture2D, texture2D.Hash, ImageType.Standard)
        {
            Size = new Vector2(texture2D.Size.X, texture2D.Size.Y)
        };
    }

    private void LoadTextureFromFile()
    {
        using var image = LoadImageFromFile(Path, FlipVertically);
        UploadTexture(image);
    }

    private void UploadTexture(StbiImage image)
    {
        var (pixelFormat, internalFormat) = GetFormat(ImageType, image.NumChannels);
        Size = new Vector2(image.Width, image.Height);
        
        Handle = _gl.GenTexture();
        _gl.BindTexture(TextureTarget, Handle);

        _gl.PixelStore(GLEnum.UnpackAlignment, 1);
        TextureFilter = _textureFilter;
        TextureWrap = _textureWrap;

        _gl.TexImage2D(TextureTarget, MipmapLevel, internalFormat, (uint)image.Width, (uint)image.Height, 0, pixelFormat, PixelType.UnsignedByte, image.Data);
        _gl.GenerateMipmap(TextureTarget);

        GlErrorUtils.CheckError("After Texture Creation");
    }

    private static StbiImage LoadImageFromFile(string path, bool flipVertically)
    {
        using var stream = File.OpenRead(path);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        Stbi.SetFlipVerticallyOnLoad(flipVertically);
        return Stbi.LoadFromMemory(memoryStream, 0);
    }
}
