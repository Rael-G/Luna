namespace Luna;

public static class Physics
{
    public static float TimeStep { get; set; } = 1 / 60.0f;

    internal static Node? Root { get; set; }

    private static float _accumulator = 0.0f;

    public static void FixedUpdate()
    {
        _accumulator += (float)Time.DeltaTime;
        while (_accumulator >= TimeStep)
        {
            Root?.InternalFixedUpdate();
            _accumulator -= TimeStep;
        }
    }
}


