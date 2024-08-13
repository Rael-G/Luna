using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public abstract class FrameBuffer<T> : RenderObject<T> where T : FrameBufferData
{
    public FrameBufferObject FBO { get; set; }

    public FrameBuffer()
    {
        FBO = new(GL, FramebufferTarget.Framebuffer);
    }

    public override void Draw()
    {
    }

    public override void Update(T data)
    {
        if (data.Bind)
        {
            Bind(data);
        }
        else
        {
            Unbind(data);
        }
    }

    protected virtual void Bind(T data)
    {
        FBO.Bind();
        var mask = ClearBufferMask.None;
        mask |= data.ClearColorBuffer? ClearBufferMask.ColorBufferBit : ClearBufferMask.None;
        mask |= data.ClearDepthBuffer? ClearBufferMask.DepthBufferBit : ClearBufferMask.None;
        mask |= data.ClearStencilBuffer? ClearBufferMask.StencilBufferBit : ClearBufferMask.None;
        GL.Clear(mask);
        GlErrorUtils.CheckError("FrameBuffer Bind");

    }

    protected virtual void Unbind(T data)
    {
        FBO.Unbind();
    }
}
