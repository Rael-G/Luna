namespace Luna;

public class Light : Node 
{
    public DirectionalLight LightSource { get; set; }

    public override void Awake()
    {
        CreateRenderObject(LightSource);

        base.Awake();
    }

    public override void LateUpdate()
    {
        LightSource.Position = Transform.GlobalPosition;
        UpdateRenderObject(LightSource);

        base.LateUpdate();
    }
}
