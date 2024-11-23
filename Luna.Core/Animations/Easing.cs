namespace Luna;

public delegate float EasingFunction(float t);

public static class Easing
{
    public static float Linear(float t) => t;
    public static float EaseIn(float t) => t * t;
    public static float EaseOut(float t) => 1 - (1 - t) * (1 - t);
    public static float EaseInOut(float t) => t < 0.5f ? 2 * t * t : 1 - MathF.Pow(-2 * t + 2, 2) / 2;
}
