using Box2DSharp.Dynamics;

namespace Luna.Box2D;

using Iteration = (int Velocity, int Position);

public class WorldManager : Node
{
    public static WorldManager Instance
    {
        get
        {
            if (_instance is null)
            {
                var worldManager = Tree.Root.FindNode<WorldManager>("WorldManager");
                if (worldManager is null)
                {
                    return _instance = new();
                }

                return worldManager;
            }

            return _instance;
        }
    }
    
    public static float PixelsPerMeter { get; set; } = 1;

    private static readonly Dictionary<int, World> Worlds = [];
    private static readonly Dictionary<int, Iteration> Iterations = [];

    private static WorldManager? _instance = null;

    private static ContactListener _contactListener = new();

    private WorldManager()
    {
    }

    public static World GetWorld(int worldIndex = 0)
    {
        if (Tree.FindNodeByUID<WorldManager>(Instance.UID) is null)
        {
            Tree.Root.AddChild(Instance);
        }

        if (Worlds.TryGetValue(worldIndex, out var world))
        {
            return world;
        }

        var newWorld = new World();
        newWorld.SetContactListener(_contactListener);
        Worlds.Add(worldIndex, newWorld);
        if (!Iterations.TryGetValue(worldIndex, out _))
        {
            Iterations[worldIndex] = (8, 3);
        }

        return newWorld;
    }

    public static void SetIterations(int worldIndex, Iteration iteration)
        => Iterations[worldIndex] = iteration;
    
    public override void FixedUpdate()
    {
        foreach(var pair in Worlds)
        {
            var velocity = Iterations[pair.Key].Velocity;
            var position = Iterations[pair.Key].Position;
            pair.Value.Step(Physics.TimeStep, velocity, position);
        }
    }
    
}