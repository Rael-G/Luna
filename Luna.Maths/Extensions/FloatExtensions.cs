namespace Luna.Maths;

public static class FloatExtensions
{
    public static float ToRadians(this float degrees)
        => (float)Math.PI / 180 * degrees;
    
    public static float ToDegrees(this float radians)
        => radians * (float)(180 / Math.PI);
    
    public static float Lerp(this float from, float to, float weight)
        => from + (to - from) * weight;

    public static float Clamp(this float f, float min, float max)
        => Math.Clamp(f, min, max);

}
