using System.Numerics;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Luna.OpenGL;

public class GlCubeMap : TextureBase
{
    public string[] Paths { get; }
    public Vector2 Size { get; private set; }
    public bool FlipVertically { get; }

    protected GlCubeMap(string[] paths, TextureFilter textureFilter, TextureWrap textureWrap, Color borderColor, int mipmaps, bool flipVertically, string hash, ImageType imageType)
        : base(textureFilter, textureWrap, borderColor, mipmaps, TextureTarget.TextureCubeMap, hash, imageType)
    {
        Paths = paths;
        FlipVertically = flipVertically;
    }

    public static GlCubeMap Create(CubeMap cubeMap)
    {
        var texture = new GlCubeMap(cubeMap.Paths, cubeMap.TextureFilter, cubeMap.TextureWrap, cubeMap.BorderColor, cubeMap.MipmapLevel, cubeMap.FlipV, cubeMap.Hash, ImageType.Linear);
        texture.LoadTextureFromFiles();
        return texture;
    }

    private void LoadTextureFromFiles()
    {
        Handle = _gl.GenTexture();
        _gl.BindTexture(TextureTarget, Handle);

        TextureFilter = _textureFilter;
        TextureWrap = _textureWrap;
        BorderColor = _borderColor;

        for (var i = 0; i < Paths.Length; i++)
        {
            using var image = LoadImageFromFile(Paths[i]);
            var (pixelFormat, internalFormat, pixelType) = GetFormat(ImageType, image.NumChannels);

            Size = new Vector2(image.Width, image.Height);
            _gl.PixelStore(GLEnum.UnpackAlignment, 1);

            _gl.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, MipmapLevel, internalFormat, (uint)image.Width, (uint)image.Height, 0, pixelFormat, pixelType, image.Data);
        }

        _gl.GenerateMipmap(TextureTarget);
        GlErrorUtils.CheckError("After CubeMap Creation");
    }

    private StbiImage LoadImageFromFile(string path)
    {
        using var stream = File.OpenRead(path);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        Stbi.SetFlipVerticallyOnLoad(FlipVertically);
        return Stbi.LoadFromMemory(memoryStream, 0);
    }

}
