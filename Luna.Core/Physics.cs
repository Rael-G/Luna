namespace Luna;

public static class Physics
{
    public static float TimeStep { get; set; } = 1 / 60.0f;

    private static float _accumulator = 0.0f;

    public static void FixedUpdate()
    {
        _accumulator += Time.DeltaTime;
        while (_accumulator >= TimeStep)
        {
            Tree.Root.InternalFixedUpdate();
            _accumulator -= TimeStep;
        }
    }
}


