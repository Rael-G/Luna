using Box2DSharp.Dynamics;

namespace Luna.Box2D;

public class StaticBody2D
    : CollisionBody2D
{
    public StaticBody2D()
    {
        Density = 0.3f;
        Restitution = 0.2f;
        Friction = 0.2f;
        BodyDef.BodyType = BodyType.StaticBody;
    }

    public override void FixedUpdate()
    {
        Body.SetTransform(Transform.Position.ToMeters(), Transform.Rotation);
        base.FixedUpdate();
    }
}
