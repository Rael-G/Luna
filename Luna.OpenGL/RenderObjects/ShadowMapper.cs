using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL.RenderObjects;

public abstract class ShadowMapper<TLight> : RenderObject<TLight>
{
    protected FrameBufferObject FBO { get; private set; }

    protected Material Material { get; private set; }

    private bool _useCulling;

    public ShadowMapper(Material material)
    {
        Material = material;
        FBO = new(GL, FramebufferTarget.Framebuffer);
        Priority = 1;
    }

    public override void Draw()
    {
        Bind();
        Injector.Get<IRenderer>().DrawQueue(Material, false);
        Unbind();
    }

    public override void Draw(IMaterial material)
    {
        // Do nothing
    }

    private void Bind()
    {
        FBO.Bind();
        GL.Clear(ClearBufferMask.DepthBufferBit);

        var window = Injector.Get<IWindow>();
        _useCulling = window.Flags.HasFlag(WindowFlags.BackFaceCulling);
        window.Flags |= WindowFlags.BackFaceCulling;
        GL.CullFace(TriangleFace.Front);

        GlErrorUtils.CheckError("ShadowMapper Bind");
    }

    private void Unbind()
    {
        GL.CullFace(TriangleFace.Back);
        if (!_useCulling)
        {
            Injector.Get<IWindow>().Flags &= ~WindowFlags.BackFaceCulling;
        }

        FBO.Unbind();

        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        GlErrorUtils.CheckError("ShadowMapper Unbind");
    }

}
