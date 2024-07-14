namespace Luna.Maths;

public static class NumericExtensions
{
    public static double ToRadians(this double degrees)
        => Math.PI / 180 * degrees;
    
    public static double ToDegrees(this double radians)
        => radians * (180 / Math.PI);
    
    public static double Lerp(this double from, double to, double weight)
        => from + (to - from) * weight;

    public static Vector3D ToRadians(this Vector3D from)
        => new(from.X.ToRadians(), from.Y.ToRadians(), from.Z.ToRadians());
    
    public static Vector3D ToDegrees(this Vector3D from)
        => new(from.X.ToDegrees(), from.Y.ToDegrees(), from.Z.ToDegrees());
}
