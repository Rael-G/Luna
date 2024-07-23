using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;

namespace Luna.Box2D;

public abstract class CollisionBody2D : Node2D
{
    // Changes after awake doest affect the body
    public int WorldIndex { get; set; }

    // Exception if not defined before CollisionBody2D.Awake()
    // A RigidBody should be rigid, changes after awake doest affect the body
    public IShape2D? Shape 
    { 
        get => _shape;
        set
        {
            _shape = value;
            FixtureDef.Shape = _shape?.CreateShape();
        }
    }

    //Changes after awake doest affect the body
    public float Density 
    { 
        get => Fixture?.Density?? FixtureDef.Density;
        set
        {
            if (Fixture is not null)
                Fixture.Density = value;
            else
                FixtureDef.Density = value;
        } 
    }

    public float Friction
    {
        get => Fixture?.Friction?? FixtureDef.Friction;
        set
        {
            if (Fixture is not null)
                Fixture.Friction = value;
            else
                FixtureDef.Friction = value;
            
        }   
    }

    public float Restitution
    {
        get => Fixture?.Restitution?? FixtureDef.Restitution;
        set
        {
            if (Fixture is not null)           
                Fixture.Restitution = value;
            else
                FixtureDef.Restitution = value;
        }
    }

    public Filter Filter
    {
        get => Fixture?.Filter?? FixtureDef.Filter;
        set
        {
            if (Fixture is not null)
                Fixture.Filter = value;
            else
                FixtureDef.Filter = value;
        }
    }

    // Is constructed at CollisionBody2D.Awake()
    protected Fixture? Fixture { get; set; } = null!;

    // Is constructed at CollisionBody2D.Awake()
    protected Body? Body { get; set; } = null!;

    protected BodyDef BodyDef;

    protected FixtureDef FixtureDef;

    private IShape2D? _shape;

    public CollisionBody2D()
    {
        BodyDef.UserData = UID;
    }

    public override void Awake()
    {
        var world = WorldManager.GetWorld(WorldIndex);
        Body = world.CreateBody(BodyDef);

        if (Shape is null)
            throw new LunaBox2DException("Shape2D should be defined before CollisionBody2D.Awake().");
        
        Fixture = Body.CreateFixture(FixtureDef);
        
        base.Awake();
    }
}

