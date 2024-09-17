using System.Numerics;
using Luna.OpenGL.Enums;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Luna.OpenGL;

public class GlCubeMap : TextureBase
{
    public string[] Paths { get; set; }

    public Vector2 Size { get; set; }

    public bool FlipV { get; }

    public override TextureWrap TextureWrap
    {
        get => _textureWrap;
        set
        {
            _textureWrap = value;
            _gl.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)TextureWrap);
            _gl.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)TextureWrap);
            _gl.TextureParameter(Handle, TextureParameterName.TextureWrapR, (int)TextureWrap);
        }
    }
    
    protected GlCubeMap(string[] paths, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps, bool flipV, string hash, ImageType imageType)
        : base(textureFilter, textureWrap, mipmaps, TextureTarget.TextureCubeMap, hash, imageType)
    {
        Paths = paths;
        ImageType = imageType;
        FlipV = flipV;
        //Hash = string.IsNullOrEmpty(hash)? GetHashCode().ToString() : hash;
    }

    public static GlCubeMap Load(string[] paths, TextureFilter textureFilter, TextureWrap textureWrap, int mipmaps,bool flipV, string hash = "", ImageType imageType = ImageType.Standard)
    {
        var texture = TextureManager.GetTexture(hash) as GlCubeMap;
        TextureManager.StartUsing(hash);
        if (texture is not null)
            return texture;
        
        texture = new GlCubeMap(paths, textureFilter, textureWrap, mipmaps, flipV, hash, imageType);
        texture.LoadTexture();
        TextureManager.Cache(hash, texture);
        GlErrorUtils.CheckError("GLCubeMap Load");
        return texture;
    }

    public static GlCubeMap Load(CubeMap cubeMap)
        => Load(cubeMap.Paths, cubeMap.TextureFilter, cubeMap.TextureWrap, 
            cubeMap.MipmapLevel, cubeMap.FlipV, cubeMap.GetHashCode().ToString());

    private void LoadTexture()
    {
        Handle = _gl.GenTexture();
        _gl.BindTexture(TextureTarget, Handle);

        TextureFilter = _textureFilter;
        TextureWrap = _textureWrap;

        for(var i = 0; i < Paths.Length; i++)
        {

            using var stream = File.OpenRead(Paths[i]);
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            Stbi.SetFlipVerticallyOnLoad(FlipV);
            using var image = Stbi.LoadFromMemory(memoryStream, 0);
            var (pFmt, iFmt) = GetFormat(ImageType, image.NumChannels);
            Size = new Vector2(image.Width, image.Height);

            _gl.PixelStore(GLEnum.UnpackAlignment, 1);
            _gl.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, MipmapLevel, iFmt, (uint)image.Width,
                (uint)image.Height, 0, pFmt, PixelType.UnsignedByte, image.Data);

            //_gl.GenerateMipmap(TextureTarget);

        }
        GlErrorUtils.CheckError("GLCubeMap LoadTexture");
    }

    public override int GetHashCode()
    {
        var paths = "";
        foreach(var path in Paths)
            paths += path;

        return (paths + FlipV).GetHashCode();
    }
}
