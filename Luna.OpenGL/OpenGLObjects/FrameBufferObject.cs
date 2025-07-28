using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class FrameBufferObject : Disposable
{
    private static readonly Stack<uint> FramebufferStack = [];
    private static readonly Stack<Vector2> ViewportStack = [];

    public uint Handle { get; }
    public FramebufferTarget FrameBufferType { get; }
    public Vector2? Viewport { get; set; }

    private readonly GL _gl;

    public FrameBufferObject(GL gl, FramebufferTarget frameBufferType)
    {
        _gl = gl;
        FrameBufferType = frameBufferType;

        Handle = _gl.GenFramebuffer();
        GlErrorUtils.CheckError("FrameBufferObject");
    }

    public void Bind()
    {
        FramebufferStack.Push(Handle);
        _gl.BindFramebuffer(FrameBufferType, Handle);
        if (Viewport is not null)
        {
            ViewportStack.Push(Viewport.Value);
            SetViewport(Viewport.Value);
        }
        GlErrorUtils.CheckError("FrameBufferObject Bind");
    }

    public void Unbind()
    {
        FramebufferStack.Pop();
        var previousFbo = FramebufferStack.Count > 0 ? FramebufferStack.Peek() : 0;
        _gl.BindFramebuffer(FrameBufferType, previousFbo);
        if (Viewport is not null)
        {
            ViewportStack.Pop();
            var previoutsVp = ViewportStack.Count > 0 ? ViewportStack.Peek() : Injector.Get<IWindow>().Size;
            SetViewport(previoutsVp);
        }
        GlErrorUtils.CheckError("FrameBufferObject Unbind");
    }

    private void SetViewport(Vector2 viewport)
    {
        _gl.Viewport(0, 0, (uint)viewport.X, (uint)viewport.Y);
        GlErrorUtils.CheckError("FrameBufferObject SetViewport");
    }

    public void AttachTexture2D(Texture2DGL texture, FramebufferAttachment attachment)
    {
        Bind();
        _gl.FramebufferTexture2D(FrameBufferType, attachment, texture.TextureTarget, texture.Handle, 
            texture.MipmapLevel);
        GlErrorUtils.CheckError("FrameBufferObject AttachTexture2D");
        Unbind();
    }

    public void AttachTexture(CubeMapGL texture, FramebufferAttachment attachment)
    {
        Bind();
        _gl.FramebufferTexture(FrameBufferType, attachment, texture.Handle, texture.MipmapLevel);
        GlErrorUtils.CheckError("FrameBufferObject AttachTexture");
        Unbind();
    }

    public void AttachRenderBuffer(RenderBufferObject rbo)
    {
        Bind();
        rbo.AttachRenderBuffer(FrameBufferType);
        Unbind();
    }

    public void CheckFrameBuffer()
    {
        Bind();
        GlErrorUtils.CheckFrameBuffer(FrameBufferType);
        Unbind();
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed)  return;

        _gl.DeleteFramebuffer(Handle);
        GlErrorUtils.CheckError("FrameBufferObject Dispose");

        base.Dispose(disposing);
    }
    
}
