namespace Luna;

public static class Physics
{
    public static float TimeStep { get; set; } = 1 / 60.0f;

    private static readonly List<ICollisionBody> Bodies = [];

    private static float _accumulator = 0.0f;

    public static void Add(ICollisionBody body)
        => Bodies.Add(body);
    
    public static void Remove(ICollisionBody body)
        => Bodies.Remove(body);

    public static void FixedUpdate()
    {
        _accumulator += Time.DeltaTime;
        while (_accumulator >= TimeStep)
        {
            foreach (var body in Bodies)
                body.FixedUpdate();
            _accumulator -= TimeStep;
        }
    }
}


