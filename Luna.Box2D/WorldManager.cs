using Box2DSharp.Dynamics;

namespace Luna.Box2D;

using Iteration = (int Velocity, int Position);

public class WorldManager : Node
{
    public static WorldManager Instance => _instance?? new();
    
    public static float PixelsPerMeter { get; set; } = 1;
    public static int VelocityIterations { get; set; } = 8;
    public static int PositionIterations { get; set; } = 3;

    private static readonly Dictionary<int, World> Worlds = [];
    private static readonly Dictionary<int, Iteration> Iterations = [];

    private static WorldManager? _instance = null;

    private static ContactListener _contactListener = new();

    private WorldManager()
    {
    }

    public static World GetWorld(int worldIndex)
    {
        if (Worlds.TryGetValue(worldIndex, out var world))
            return world;

        var newWorld = new World();
        newWorld.SetContactListener(_contactListener);
        Worlds.Add(worldIndex, newWorld);
        if (!Iterations.TryGetValue(worldIndex, out _))
            Iterations[worldIndex] = (8, 3);

        return newWorld;
    }

    public static void SetIterations(int worldIndex, Iteration iteration)
        => Iterations[worldIndex] = iteration;
    
    public void FixedUpdate()
    {
        foreach(var pair in Worlds)
        {
            var velocity = Iterations[pair.Key].Velocity;
            var position = Iterations[pair.Key].Position;
            pair.Value.Step(Physics.TimeStep, velocity, position);
        }
    }
    
}