using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class FrameBufferObject : Disposable
{
    public uint Handle { get; }
    private readonly GL _gl;
    public FramebufferTarget FrameBufferType { get; }

    public FrameBufferObject(GL gl, FramebufferTarget frameBufferType)
    {
        _gl = gl;
        FrameBufferType = frameBufferType;

        Handle = _gl.GenFramebuffer();
        GlErrorUtils.CheckError("FrameBufferObject");
    }

    public void Bind()
    {
        _gl.BindFramebuffer(FrameBufferType, Handle);
        GlErrorUtils.CheckError("FrameBufferObject Bind");
    }

    public void Unbind()
    {
        _gl.BindFramebuffer(FrameBufferType, 0);
        GlErrorUtils.CheckError("FrameBufferObject Unbind");
    }

    public void AttachTexture2D(GlTexture2D texture, FramebufferAttachment attachment)
    {
        Bind();
        _gl.FramebufferTexture2D(FrameBufferType, attachment, texture.TextureTarget, texture.Handle, texture.MipmapLevel);
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
