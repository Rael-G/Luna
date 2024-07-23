using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics.Contacts;

namespace Luna.Box2D;

public interface IOnCollision
{
    void OnCollisionEnter(CollisionBody2D body, Contact contact);

    void OnCollisionExit(CollisionBody2D body, Contact contact);

    void OnTriggerEnter(TriggerArea2D area);

    void OnTriggerExit(TriggerArea2D area);

    void OnCollisionPreSolve(CollisionBody2D body, Contact contact, in Manifold impulse);

    void OnCollisionPostSolve(CollisionBody2D body, Contact contact, in ContactImpulse oldManifold);
}
