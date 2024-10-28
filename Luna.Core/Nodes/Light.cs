namespace Luna;

public class Light : Node 
{
    public DirectionalLight LightSource { get; set; }

    public override void Awake()
    {
        Injector.Get<ILightEmitter>().Add(UID, LightSource);
        CreateRenderObject(LightSource);

        base.Awake();
    }

    public override void LateUpdate()
    {
        LightSource.Position = Transform.GlobalPosition;
        UpdateRenderObject(LightSource);

        base.LateUpdate();
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        Injector.Get<ILightEmitter>().Remove(UID);

        base.Dispose(disposing);
    }
}
