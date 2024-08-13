using Silk.NET.OpenGL;

namespace Luna.OpenGL.RenderObjects;

public class RenderBufferObject : Disposable
{
    private readonly uint _handle;
    private readonly RenderbufferTarget _renderBufferType;

    private readonly FramebufferAttachment _frameBufferAttachment;
    private readonly GL _gl;

    public RenderBufferObject(GL gl, RenderbufferTarget renderBufferType, InternalFormat internalFormat, FramebufferAttachment framebufferAttachment, uint width, uint height)
    {
        _gl = gl;
        _renderBufferType = renderBufferType;
        _frameBufferAttachment = framebufferAttachment;

        _handle = _gl.GenRenderbuffer();
        Bind();
        _gl.RenderbufferStorage(_renderBufferType, internalFormat, width, height);
        GlErrorUtils.CheckError("RenderBufferObject");
    }

    public void Bind()
    {
        _gl.BindRenderbuffer(_renderBufferType, _handle);
        GlErrorUtils.CheckError("RenderBufferObject Bind");
    }

    public void Unbind()
    {
        _gl.BindRenderbuffer(_renderBufferType, 0);
        GlErrorUtils.CheckError("RenderBufferObject Unbind");
    }

    public void AttachRenderBuffer(FramebufferTarget frameBufferType)
    {
        Bind();
        _gl.FramebufferRenderbuffer(frameBufferType, _frameBufferAttachment, _renderBufferType, _handle);
        GlErrorUtils.CheckError("RenderBufferObject AttachRenderBuffer");
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed)  return;

        _gl.DeleteRenderbuffer(_handle);
        
        GlErrorUtils.CheckError("RenderBufferObject Dispose");

        base.Dispose(disposing);
    }
}
