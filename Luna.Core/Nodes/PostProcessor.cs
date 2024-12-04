using System.Numerics;

namespace Luna;

public class PostProcessor : Node
{
    public IMaterial Material { get; set; }

    public Vector2 Resolution { get; set; } = Window.Size;

    public int Samples { get; set; }

    public override void Awake()
    {
        CreateRenderObject(new PostProcessorData(){ Material = Material, Resolution = Resolution, Samples = Samples });

        base.Awake();
    }

    public override void Update()
    {
        UpdateRenderObject(new PostProcessorData(){ Material = Material, Resolution = Resolution, Samples = Samples });

        base.Update();
    }
}
