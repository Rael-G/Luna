using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class CubeMapGL : Texture
{
    public CubeMapGL(GL gl, string[] paths, bool flipVertically, ImageType imageType, FilterMode filterMode, WrapMode wrapMode, int mipmapLevel = 0) 
        : base(gl, mipmapLevel, TextureTarget.TextureCubeMap)
    {
        Bind();

        for (var i = 0; i < paths.Length; i++)
        {
            using var image = LoadImageFromFile(paths[i], flipVertically);
            var (pixelFormat, internalFormat, pixelType) = GetFormat(imageType, image.NumChannels);
            GenerateCubemap(i, new Vector2(image.Width, image.Height), pixelFormat, internalFormat, 
                pixelType, image.Data);
        }

        SetFilterMode(filterMode);
        SetWrapMode(wrapMode);

        Unbind();
    }

    public CubeMapGL(GL gl, Vector2 size, ImageType imageType, FilterMode filterMode, WrapMode wrapMode,
        int mipmapLevel = 0) 
        : base(gl, mipmapLevel, TextureTarget.TextureCubeMap)
    {
        Bind();

        for (var i = 0; i < 6; i++)
        {
            var (pixelFormat, internalFormat, pixelType) = GetFormat(imageType, 3);
            GenerateCubemap(i, size, pixelFormat, internalFormat, pixelType, new ReadOnlySpan<byte>());
        }
        SetFilterMode(filterMode);
        SetWrapMode(wrapMode);

        Unbind();
    }

    public void GenerateCubemap(int iteration, Vector2 size, PixelFormat pixelFormat, 
        InternalFormat internalFormat, PixelType pixelType, ReadOnlySpan<byte> data)
    {
        Gl.PixelStore(GLEnum.UnpackAlignment, 1);
        Gl.TexImage2D(TextureTarget.TextureCubeMapPositiveX + iteration, MipmapLevel, internalFormat, 
            (uint)size.X, (uint)size.Y, 0, pixelFormat, pixelType, data);
        GlErrorUtils.CheckError("CubeMapGL GenerateTexture2D");
    }

    protected override void SetWrapMode(WrapMode wrapMode)
    {
        Gl.TextureParameter(Handle, TextureParameterName.TextureWrapR, (int)wrapMode);
        GlErrorUtils.CheckError("CubeMapGL SetTextureWrap");

        base.SetWrapMode(wrapMode);
    }
}
