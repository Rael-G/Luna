using System.Numerics;
using Luna.Maths;

namespace Luna;

public class Light<T> : Node 
    where T : DirectionalLight
{
    public T LightSource { get; } = new DirectionalLight().CreateLight<T>();

    public override void Awake()
    {
        Injector.Get<ILightEmitter>().Add(UID, LightSource);
        base.Awake();
    }

    public override void LateUpdate()
    {
        LightSource.Position = Transform.GlobalPosition;
        LightSource.Direction = (-Vector3.UnitZ).Transform(Transform.Quaternion);
        base.LateUpdate();
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        Injector.Get<ILightEmitter>().Remove(UID);

        base.Dispose(disposing);
    }
}
