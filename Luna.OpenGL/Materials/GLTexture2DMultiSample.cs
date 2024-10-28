using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class GlTexture2DMultiSample : GlTexture2D
{
    public int Samples { get; set; }

    private GlTexture2DMultiSample(Vector2 size, TextureFilter filter, TextureWrap wrap, Color borderColor, string hash, int samples)
        : base(string.Empty, filter, wrap, borderColor, 0, false, TextureTarget.Texture2DMultisample, hash, ImageType.Linear)
    {
        Samples = Math.Clamp(samples, 2, _gl.GetInteger(GLEnum.MaxSamples));
        Size = size;
    }

    public static GlTexture2DMultiSample Create(Texture2D texture2D, int samples)
    {
        var texture = new GlTexture2DMultiSample(texture2D.Size, texture2D.TextureFilter, texture2D.TextureWrap, texture2D.BorderColor, texture2D.Hash, samples);

        texture.LoadMultisampleTexture();
        return texture;
    }

    private void LoadMultisampleTexture()
    {
        Handle = _gl.GenTexture();

        Bind();

        _gl.TexImage2DMultisample(TextureTarget, (uint)Samples, InternalFormat.Rgb8, 
            (uint)Size.X, (uint)Size.Y, true);
        GlErrorUtils.CheckError("Multisample Texture Creation");

        Unbind();

    }
}
