using System.Numerics;

namespace Luna.Maths;

public static class Vector2Extensions
{
    public static Vector3 ToVector3(this Vector2 vector)
        => new(vector.X, vector.Y, 0.0f);

    public static Matrix ToMatrix(this Vector2 vector)
        => new(3, 1, [vector.X , vector.Y , 0.0f] );

    public static Matrix ToMatrix(this Vector2[] vectors)
    {
        var matrix = new Matrix(3, vectors.Length);
        for (int i = 0; i < vectors.Length; i++)
        {
            matrix[i, 0] = vectors[i].X;
            matrix[i, 1] = vectors[i].Y;
            matrix[i, 2] = 0f;
        }
        return matrix;
    }

    public static Vector2? ToVector2(this Matrix matrix)
    {
        try
        {
            return new(matrix[0,0], matrix[1,0]);
        }
        catch
        {
            return null;
        }
    }

    public static Vector2 Normalize(this Vector2 vector)
        => Vector2.Normalize(vector);
    
    public static float Dot(this Vector2 vector, Vector2 other)
        => Vector2.Dot(vector, other);
    
    public static float Cross(this Vector2 vector, Vector2 other)
        => vector.X * other.Y - vector.Y * other.X;
    
    public static float AngleTo(this Vector2 vector, Vector2 to)
    {   
        var cosTheta = vector.Dot(to) / (vector.Length() * to.Length());
        var angle = (float)Math.Acos(cosTheta);
        return vector.Cross(to) > 0? angle : -angle;
    }

    public static Vector2 Rotate(this Vector2 vector, float angle)
        => (Vector2)(Transformations.RotationMatrix(angle, new Vector3(0, 0, 1)) * vector.ToMatrix()).ToVector2()!;
    
    public static  Vector2 RotateTo(this Vector2 vector, Vector2 to)
        => vector.Rotate(vector.AngleTo(to));

    public static Vector2 Scale(this Vector2 vector, Vector2 other)
        => (Vector2)(other.ToMatrix().Diagonal() * vector.ToMatrix()).ToVector2()!;

    public static Vector2 Shear(this Vector2 vector, Vector2 shearFactor)
        => (Vector2)(Transformations.ShearMatrix(shearFactor.ToVector3(), new Vector3(0, 0, 1)) * vector.ToMatrix()).ToVector2()!;
    
    public static Vector2 Lerp(this Vector2 from, Vector2 to, float weight)
        => Vector2.Lerp(from, to, weight);

    public static float Distance(this Vector2 from, Vector2 to)
        => Vector2.Distance(from, to);

    public static Vector2 Abs(this Vector2 vector)
        => Vector2.Abs(vector);

    public static Vector2 Clamp(this Vector2 vector, Vector2 min, Vector2 max)
        => Vector2.Clamp(vector, min, max);

    public static float DistanceSquared(this Vector2 from, Vector2 to)
        => Vector2.DistanceSquared(from, to);

    public static Vector2 Reflect(this Vector2 vector, Vector2 normal)
        => Vector2.Reflect(vector, normal);

    public static Vector2 SquareRoot(this Vector2 vector)
        => Vector2.SquareRoot(vector);

}