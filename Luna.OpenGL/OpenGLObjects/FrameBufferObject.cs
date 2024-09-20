using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class FrameBufferObject : Disposable
{
    public uint Handle { get; }
    private readonly GL _gl;
    private readonly FramebufferTarget _frameBufferType;

    public FrameBufferObject(GL gl, FramebufferTarget frameBufferType)
    {
        _gl = gl;
        _frameBufferType = frameBufferType;

        Handle = _gl.GenFramebuffer();
        GlErrorUtils.CheckError("FrameBufferObject");
    }

    public void Bind()
    {
        _gl.BindFramebuffer(_frameBufferType, Handle);
        GlErrorUtils.CheckError("FrameBufferObject Bind");
    }

    public void Unbind()
    {
        _gl.BindFramebuffer(_frameBufferType, 0);
        GlErrorUtils.CheckError("FrameBufferObject Unbind");
    }

    public void AttachTexture2D(GlTexture2D texture)
    {
        Bind();
        var attachment = AttachmentType(texture.ImageType);
        _gl.FramebufferTexture2D(_frameBufferType, attachment, texture.TextureTarget, texture.Handle, texture.MipmapLevel);
        GlErrorUtils.CheckFrameBuffer(_frameBufferType, "FrameBufferObject AttachTexture2D");
        Unbind();
    }

    public void AttachRenderBuffer(RenderBufferObject rbo)
    {
        Bind();
        rbo.AttachRenderBuffer(_frameBufferType);
        Unbind();
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed)  return;

        _gl.DeleteFramebuffer(Handle);
        GlErrorUtils.CheckError("FrameBufferObject Dispose");

        base.Dispose(disposing);
    }

    private FramebufferAttachment AttachmentType(ImageType imageType)
        => imageType switch
        {
            ImageType.DepthMap => FramebufferAttachment.DepthAttachment,
            ImageType.StencilMap => FramebufferAttachment.StencilAttachment,
            ImageType.DepthStencilMap => FramebufferAttachment.DepthStencilAttachment,
            _ => FramebufferAttachment.ColorAttachment0,
        };
    
}
