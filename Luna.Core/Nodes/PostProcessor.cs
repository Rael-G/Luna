using System.Numerics;

namespace Luna;

public class PostProcessor : Node
{
    public ShaderSource[] Shaders { get; set; } = [];

    public Vector2 Resolution { get; set; } = Window.Size;

    public bool MSAA { get; set; }

    public override void Awake()
    {
        CreateRenderObject(new PostProcessorData(){ Shaders = Shaders, Resolution = Resolution, MSAA = MSAA });

        base.Awake();
    }

    internal override void Draw()
    {
        if (Invisible)  return;

        var map = Injector.Get<IRenderer>();
        map.Draw(UID);

        foreach (var child in Children)
            child.Draw();
    }
}
