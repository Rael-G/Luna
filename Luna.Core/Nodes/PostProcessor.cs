using System.Numerics;

namespace Luna;

public class PostProcessor : Node
{
    public ShaderSource[] Shaders { get; set; } = [];

    public Vector2 Resolution { get; set; } = Window.Size;

    public override void Awake()
    {
        CreateRenderObject(new PostProcessorData(){ Shaders = Shaders, Resolution = Resolution });

        base.Awake();
    }

    internal override void Draw()
    {
        if (Invisible)  return;

        UpdateRenderObject(new PostProcessorData{ Resolution = Resolution, Bind = true, ClearColorBuffer = true, ClearDepthBuffer = true});
        foreach (var child in Children)
            child.Draw();
        UpdateRenderObject(new PostProcessorData{ Resolution = Resolution, Bind = false });
    
        Injector.Get<IRenderer>().Draw(UID);
    }
}
