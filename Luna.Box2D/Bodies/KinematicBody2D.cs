using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;

namespace Luna.Box2D;

public class KinematicBody2D 
    : CollisionBody2D, IOnCollision
{
    public KinematicBody2D()
    {
        BodyDef.BodyType = BodyType.KinematicBody;
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
