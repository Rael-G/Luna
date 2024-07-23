using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Luna.Core;

namespace Luna.Box2D;

public class ContactListener : IContactListener
{
    public void BeginContact(Contact contact)
    {
        var (bodyA, bodyB) = GetBodies(contact);

        if (bodyA is IOnCollision colA)
        {
            if (bodyB is TriggerArea2D trigger)
            {
                colA.OnTriggerEnter(trigger);
                return;
            }

            colA.OnCollisionEnter(bodyB, contact);
        }

        if (bodyB is IOnCollision colB)
        {
            if (bodyA is TriggerArea2D trigger)
            {
                colB.OnTriggerEnter(trigger);
                return;
            }

            colB.OnCollisionEnter(bodyA, contact);
        }
    }

    public void EndContact(Contact contact)
    {
        var (bodyA, bodyB) = GetBodies(contact);

        if (bodyA is IOnCollision colA)
        {
            if (bodyB is TriggerArea2D trigger)
            {
                colA.OnTriggerExit(trigger);
                return;
            }

            colA.OnCollisionExit(bodyB, contact);
        }

        if (bodyB is IOnCollision colB)
        {
            if (bodyA is TriggerArea2D trigger)
            {
                colB.OnTriggerExit(trigger);
                return;
            }

            colB.OnCollisionExit(bodyA, contact);
        }
    }

    public void PreSolve(Contact contact, in Manifold oldManifold)
    {
        var (bodyA, bodyB) = GetBodies(contact);

        if (bodyA is IOnCollision colA)
        {
            colA.OnCollisionPreSolve(bodyB, contact, oldManifold);
        }

        if (bodyB is IOnCollision colB)
        {
            colB.OnCollisionPreSolve(bodyA, contact, oldManifold);
        }
    }

    public void PostSolve(Contact contact, in ContactImpulse impulse)
    {
        var (bodyA, bodyB) = GetBodies(contact);

        if (bodyA is IOnCollision colA)
        {
            colA.OnCollisionPostSolve(bodyB, contact, impulse);
        }

        if (bodyB is IOnCollision colB)
        {
            colB.OnCollisionPostSolve(bodyA, contact, impulse);
        }
    }

    private (CollisionBody2D bodyA, CollisionBody2D bodyB) GetBodies(Contact contact)
    {
        var uidA = (string)contact.FixtureA.Body.UserData;
        var uidB = (string)contact.FixtureB.Body.UserData;

        var bodyA = Tree.FindNodeByUID<CollisionBody2D>(uidA)!;
        var bodyB = Tree.FindNodeByUID<CollisionBody2D>(uidB)!;

        return (bodyA, bodyB);
    }
}
