namespace Luna;

public class Tween(float from, float to, int duration, EasingFunction? easingFunction = null) : AnimationBase(from, to)
{
    public int Duration { get; set; } = duration;
    
    private readonly EasingFunction? _easingFunction = easingFunction;

    public override void Update(float deltaTime)
    {
        if (IsCompleted) return;

        ElapsedTime += deltaTime * 1000;
        Weight = Math.Clamp(ElapsedTime / Duration, 0f, 1f);
        Current = Interpolate(From, To, Weight, _easingFunction);

        if (Weight >= 1f)
        {
            IsCompleted = true;
            Current = To;
        }
    }
}