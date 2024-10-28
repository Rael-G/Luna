using System.Numerics;

namespace Luna.Maths;

public static class Vector3Extensions
{
    // public static Matrix ToMatrix(this Vector3 Vector)
    //     => new(4, 1, [Vector.X , Vector.Y, Vector.Z, 1.0f]);

    // public static Matrix ToMatrix(this Vector3[] vectors)
    // {
    //     var matrix = new Matrix(3, vectors.Length);
    //     for (int i = 0; i < vectors.Length; i++)
    //     {
    //         matrix[i, 0] = vectors[i].X;
    //         matrix[i, 1] = vectors[i].Y;
    //         matrix[i, 2] = vectors[i].Z;
    //     }
    //     return matrix;
    // }

    // public static Vector3? ToVector3(this Matrix matrix)
    // {
    //     try
    //     {
    //         return new(matrix[0,0], matrix[1,0], matrix[2,0]);
    //     }
    //     catch
    //     {
    //         return null;
    //     }
    // }

    public static float[] ToFloatArray(this Vector3 vector)
        => [vector.X, vector.Y, vector.Z];

    public static Vector2 ToVector2(this Vector3 vector)
        => new(vector.X, vector.Y);
    
    public static Quaternion ToQuaternion(this Vector3 vector)
        => Quaternion.Normalize(Quaternion.CreateFromYawPitchRoll(vector.Y, vector.X, vector.Z));

    public static Vector3 ToVector3(this Quaternion quaternion)
        => new (quaternion.X, quaternion.Y, quaternion.Z);

    public static Vector3 Normalize(this Vector3 vector)
    {
        if (vector == Vector3.Zero)
            return Vector3.Zero;

        var result = Vector3.Normalize(vector);
        return result;
    }
    
    public static float Dot(this Vector3 vector, Vector3 other)
        => Vector3.Dot(vector, other);
    
    public static Vector3 Cross(this Vector3 vector, Vector3 other)
        => Vector3.Cross(vector, other);

    public static float Mix(this Vector3 vector, Vector3 otherA, Vector3 otherB)
        => vector.Dot(otherA.Cross(otherB));
    
    public static float AngleTo(this Vector3 vector, Vector3 other)
        => (float)Math.Atan2(vector.Cross(other).Length(), vector.Dot(other));

    public static Vector3 Rotate(this Vector3 vector, float angle, Vector3 axis)
        => Vector3.Transform(axis.Normalize(), Quaternion.CreateFromAxisAngle(axis, angle));
    
    public static Vector3 RotateDegress(this Vector3 vector, float angle, Vector3 axis)
        => Rotate(vector, angle.ToRadians(), axis);
    
    public static Vector3 RotateTo(this Vector3 vector, Vector3 other)
    {
        Vector3 cross = vector.Cross(other);

        if (cross.LengthSquared() < 1e-6)
        {
            float dot = vector.Dot(other);

            if (dot > 0)
                return vector;
            else
            {
                var axis = vector.Cross(Vector3.UnitX).Normalize();
                if (axis.LengthSquared() < 1e-6)
                    axis = vector.Cross(Vector3.UnitY).Normalize();
        
                return vector.Rotate((float)Math.PI, axis);
            }
        }

        return vector.Rotate(vector.AngleTo(other), cross.Normalize());
    }

    public static Vector3 LookTo(this Vector3 rotation, Vector3 position, Vector3 otherPosition)
    {
        var direction = (otherPosition - position).Normalize();
        if (direction.LengthSquared() < 1e-6)
            return rotation;

        return rotation.RotateTo(direction);
    }

    public static Vector3 Scale(this Vector3 vector, Vector3 other)
        => vector.Transform(other.CreateScale());

    // public static Vector3 Shear(this Vector3 vector, Vector3 shearFactor, Vector3 axis)
    //     => (Vector3)(Transformations.ShearMatrix(shearFactor, axis) * vector.ToMatrix()).ToVector3()!;
    
    public static Vector3 Lerp(this Vector3 from, Vector3 to, float weight)
        => Vector3.Lerp(from, to, weight);

    public static float Distance(this Vector3 from, Vector3 to)
        => Vector3.Distance(from, to);

    public static Vector3 Abs(this Vector3 vector)
        => Vector3.Abs(vector);

    public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
        => Vector3.Clamp(vector, min, max);

    public static float DistanceSquared(this Vector3 from, Vector3 to)
        => Vector3.DistanceSquared(from, to);

    public static Vector3 Reflect(this Vector3 vector, Vector3 normal)
        => Vector3.Reflect(vector, normal);

    public static Vector3 SquareRoot(this Vector3 vector)
        => Vector3.SquareRoot(vector);

    public static Vector3 ToRadians(this Vector3 from)
        => new(from.X.ToRadians(), from.Y.ToRadians(), from.Z.ToRadians());
    
    public static Vector3 ToDegrees(this Vector3 from)
        => new(from.X.ToDegrees(), from.Y.ToDegrees(), from.Z.ToDegrees());

    public static Matrix4x4 CreateTranslation(this Vector3 vector)
        => Matrix4x4.CreateTranslation(vector);

    public static Matrix4x4 CreateScale(this Vector3 vector)
        => Matrix4x4.CreateScale(vector);

    public static Vector3 Transform(this Vector3 vector, Matrix4x4 matrix)    
        => Vector3.Transform(vector, matrix);

    public static Vector3 Transform(this Vector3 vector, Quaternion quaternion)
        => Vector3.Transform(vector, quaternion);
}
