using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Luna.Maths;

namespace Luna.Box2D;

public class DynamicBody2D 
    : CollisionBody2D, IOnCollision
{
    public bool FixedRotation
    { 
        get => Body?.FixedRotation?? BodyDef.FixedRotation;
        set
        {
            if (Body is not null)
                Body.FixedRotation = value;
            else
                BodyDef.FixedRotation = value;
        }
    }

    public DynamicBody2D()
    {
        Density = 0.3f;
        Restitution = 0.2f;
        Friction = 0.2f;
        BodyDef.BodyType = BodyType.DynamicBody;

    }

    public override void Start()
    {
        Body.SetTransform(Transform.Position.ToMeters().ToVector2(), Transform.Rotation.Z);
        base.Start();
    }

    public override void FixedUpdate()
    {
        Transform.Position = Body!.GetPosition().ToPixels().ToVector3();
        Transform.Rotation = new (Transform.Rotation.X, Transform.Rotation.Y, Body.GetAngle());

        base.FixedUpdate();
    }

    public virtual void OnCollisionEnter(CollisionBody2D body, Contact contact)
    {

    }

    public virtual void OnCollisionExit(CollisionBody2D body, Contact contact)
    {

    }

    public virtual void OnTriggerEnter(TriggerArea2D area)
    {

    }

    public virtual void OnTriggerExit(TriggerArea2D area)
    {

    }

    public virtual void OnCollisionPreSolve(CollisionBody2D body, Contact contact, in Manifold impulse)
    {

    }

    public virtual void OnCollisionPostSolve(CollisionBody2D body, Contact contact, in ContactImpulse oldManifold)
    {
        
    }
}
