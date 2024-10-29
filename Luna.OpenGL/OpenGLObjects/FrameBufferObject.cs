using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class FrameBufferObject : Disposable
{
    private static readonly Stack<uint> FramebufferStack = [];
    private static readonly Stack<Vector2> ViewportStack = [];

    public uint Handle { get; }
    public FramebufferTarget FrameBufferType { get; }
    public Vector2 Viewport { get; set; }

    private readonly GL _gl;

    public FrameBufferObject(GL gl, FramebufferTarget frameBufferType, Vector2 viewport)
    {
        _gl = gl;
        FrameBufferType = frameBufferType;
        Viewport = viewport;

        Handle = _gl.GenFramebuffer();
        GlErrorUtils.CheckError("FrameBufferObject");
    }

    public void Bind()
    {
        FramebufferStack.Push(Handle);
        ViewportStack.Push(Viewport);
        _gl.BindFramebuffer(FrameBufferType, Handle);
        SetViewport(Viewport);
        GlErrorUtils.CheckError("FrameBufferObject Bind");
    }

    public void Unbind()
    {
        FramebufferStack.Pop();
        ViewportStack.Pop();
        var previousFbo = FramebufferStack.Count > 0 ? FramebufferStack.Peek() : 0;
        var previoutsVp = ViewportStack.Count > 0 ? ViewportStack.Peek() : Injector.Get<IWindow>().Size;
        _gl.BindFramebuffer(FrameBufferType, previousFbo);
        SetViewport(previoutsVp);
        GlErrorUtils.CheckError("FrameBufferObject Unbind");
    }

    private void SetViewport(Vector2 viewport)
    {
        _gl.Viewport(0, 0, (uint)viewport.X, (uint)viewport.Y);
    }

    public void AttachTexture2D(Texture2DGL texture, FramebufferAttachment attachment)
    {
        Bind();
        _gl.FramebufferTexture2D(FrameBufferType, attachment, texture.TextureTarget, texture.Handle, 
            texture.MipmapLevel);
        Unbind();
    }

    public void AttachTexture(Texture2DGL texture, FramebufferAttachment attachment)
    {
        Bind();
        _gl.FramebufferTexture(FrameBufferType, attachment, texture.Handle, texture.MipmapLevel);
        Unbind();
    }

    public void AttachRenderBuffer(RenderBufferObject rbo)
    {
        Bind();
        rbo.AttachRenderBuffer(FrameBufferType);
        Unbind();
    }

    public void CheckFrameBuffer(string location)
    {
        Bind();
        GlErrorUtils.CheckFrameBuffer(FrameBufferType, location);
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
