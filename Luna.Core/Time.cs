using System.Diagnostics;

namespace Luna;

public static class Time
{
    public static float ElapsedTime { get => (float)_stopwatch.Elapsed.TotalSeconds; }

    public static float DeltaTime { get; private set; }

    public static float FixedDeltaTime => Physics.TimeStep;

    public static int FrameRate { get; set; } = int.MaxValue;

    private static readonly Stopwatch _stopwatch = new();

    private static float _previousFrameTime;

    internal static void StartTimer()
    {
        if (!_stopwatch.IsRunning)
        {
            _stopwatch.Start();
            _previousFrameTime = (float)_stopwatch.Elapsed.TotalSeconds;
        }
    }

    internal static void SetDeltaTime()
    {
        DeltaTime = ElapsedTime - _previousFrameTime;
        _previousFrameTime = ElapsedTime;
    }

    internal static void NextFrame()
    {
        var frameDuration = 1.0 / FrameRate;
        var targetTime = _previousFrameTime + frameDuration;

        while (ElapsedTime < targetTime)
        {
            Thread.Yield();
        }
    }
}
