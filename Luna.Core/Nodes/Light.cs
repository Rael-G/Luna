using System.Numerics;
using Luna.Maths;

namespace Luna;

public class Light(DirectionalLight lightSource) : Node 
{
    public DirectionalLight LightSource { get; } = lightSource;
    
    public override void Awake()
    {
        Injector.Get<ILightEmitter>().Add(UID, LightSource);
        base.Awake();
    }

    public override void LateUpdate()
    {
        LightSource.Position = Transform.GlobalPosition;
        base.LateUpdate();
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        Injector.Get<ILightEmitter>().Remove(UID);

        base.Dispose(disposing);
    }
}
