namespace Luna;

public class PostProcessor : Node
{
    public ShaderSource[] Shaders { get; set; } = [];

    public override void Awake()
    {
        CreateRenderObject(new PostProcessorData(){ Shaders = Shaders });

        base.Awake();
    }

    internal override void Draw()
    {
        if (Invisible)  return;

        UpdateRenderObject(new PostProcessorData{ Bind = true, ClearColorBuffer = true, ClearDepthBuffer = true });
        foreach (var child in Children)
            child.Draw();
        UpdateRenderObject(new PostProcessorData{ Bind = false });
    
        Injector.Get<IRenderer>().Draw(UID);
    }
}
