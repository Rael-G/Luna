using System.Numerics;

namespace Luna.Maths;

public static class QuaternionExtensions
{
    public static Matrix4x4 ToMatrix4x4(this Quaternion quaternion)
        => Matrix4x4.CreateFromQuaternion(quaternion);
}
