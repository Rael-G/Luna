namespace Luna;

public static class Time
{
    public static float DeltaTime { get; }

    public static float TimeStep { get; set; } = 1 / 60.0f;

    internal static float StepAccumulator { get; set; } = 0.0f;

}
