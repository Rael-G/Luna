using System.Numerics;

namespace Luna.Maths;

public static class NumericExtensions
{
    public static float ToRadians(this float degrees)
        => (float)Math.PI / 180 * degrees;
    
    public static float ToDegrees(this float radians)
        => radians * (float)(180 / Math.PI);
    
    public static float Lerp(this float from, float to, float weight)
        => from + (to - from) * weight;

    public static float Clamp(this float f, float min, float max)
        => Math.Clamp(f, min, max);

    public static Matrix ToMatrix(this Quaternion quaternion)
    {
        float x2 = quaternion.X + quaternion.X;
        float y2 = quaternion.Y + quaternion.Y;
        float z2 = quaternion.Z + quaternion.Z;
        float xx = quaternion.X * x2;
        float yy = quaternion.Y * y2;
        float zz = quaternion.Z * z2;
        float xy = quaternion.X * y2;
        float xz = quaternion.X * z2;
        float yz = quaternion.Y * z2;
        float wx = quaternion.W * x2;
        float wy = quaternion.W * y2;
        float wz = quaternion.W * z2;

        var matrix = new Matrix(4, 4);

        matrix[0, 0] = 1.0f - (yy + zz);
        matrix[0, 1] = xy + wz;
        matrix[0, 2] = xz - wy;
        matrix[0, 3] = 0.0f;

        matrix[1, 0] = xy - wz;
        matrix[1, 1] = 1.0f - (xx + zz);
        matrix[1, 2] = yz + wx;
        matrix[1, 3] = 0.0f;

        matrix[2, 0] = xz + wy;
        matrix[2, 1] = yz - wx;
        matrix[2, 2] = 1.0f - (xx + yy);
        matrix[2, 3] = 0.0f;

        matrix[3, 0] = 0.0f;
        matrix[3, 1] = 0.0f;
        matrix[3, 2] = 0.0f;
        matrix[3, 3] = 1.0f;

        return matrix;
    }
}
