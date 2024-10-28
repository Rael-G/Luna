using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class DepthMap : Disposable
{
    GL _gl;
    FrameBufferObject _fbo { get; set; }
    bool useCulling;

    Vector2 _size;
    public Texture2D DepthMapTexture { get; private set; }

    public DepthMap(Vector2 size, GL gl)
    {
        _gl = gl;
        _fbo = new(gl, FramebufferTarget.Framebuffer);
        CreateDepthMap(size);
    }

    public void Bind()
    {
        _gl.Viewport(0, 0, (uint)_size.X, (uint)_size.Y);
        _fbo.Bind();
        _gl.Clear(ClearBufferMask.DepthBufferBit);

        var window = Injector.Get<IWindow>();
        useCulling = window.Flags.HasFlag(WindowFlags.BackFaceCulling);
        window.Flags |= WindowFlags.BackFaceCulling;
        _gl.CullFace(TriangleFace.Front);

        GlErrorUtils.CheckError("DepthMap Bind");
    }

    public void Unbind()
    {
        _gl.CullFace(TriangleFace.Back);
        if (!useCulling)
        {
            Injector.Get<IWindow>().Flags &= ~WindowFlags.BackFaceCulling;
        }

        _fbo.Unbind();

        var size = Injector.Get<IWindow>().Size;
        _gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
        _gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        GlErrorUtils.CheckError("DepthMap Unbind");
    }

    public void CreateDepthMap(Vector2 size)
    {
        _size = size;
        TextureManager.Get(DepthMapTexture.Hash)?.Dispose();
        
        var width = (uint)size.X;
        var height = (uint)size.Y;

        DepthMapTexture = new Texture2D()
        {
            Size = new Vector2(width, height),
            TextureFilter = TextureFilter.Nearest,
            TextureWrap = TextureWrap.ClampToBorder,
            Hash = Guid.NewGuid().ToString(),
            ImageType = ImageType.DeathMap
        };

        var depthMapTexture = GlTexture2D.Create(DepthMapTexture);
        TextureManager.Cache(DepthMapTexture.Hash, depthMapTexture);

        _fbo.AttachTexture2D(depthMapTexture, FramebufferAttachment.DepthAttachment);
        _fbo.Bind();
        _gl.DrawBuffer(DrawBufferMode.None);
        _gl.ReadBuffer(ReadBufferMode.None);
        _fbo.Unbind();
    
        _fbo.CheckFrameBuffer("DepthMap");
    }

}
