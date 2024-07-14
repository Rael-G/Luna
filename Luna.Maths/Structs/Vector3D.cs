namespace Luna.Maths;

public struct Vector3D(double x, double y, double z)
{
    public double X { get; set; } = x;

    public double Y { get; set; } = y;

    public double Z { get; set; } = z;

    public readonly double Length 
    { 
        get => Math.Sqrt( X * X + Y * Y + Z * Z);
    }

    /// <summary>
    /// 0, 0, 0
    /// </summary>
    public static readonly Vector3D Zero = new(0, 0, 0);

    /// <summary>
    /// 1, 0, 0
    /// </summary>
    public static readonly Vector3D Right = new(1, 0, 0);

    /// <summary>
    /// -1, 0, 0
    /// </summary>
    public static readonly Vector3D Left = new(-1, 0, 0);

    /// <summary>
    /// 0, 1, 0
    /// </summary>
    public static readonly Vector3D Up = new(0, 1, 0);

    /// <summary>
    /// 0, -1, 0
    /// </summary>
    public static readonly Vector3D Down = new(0, -1, 0);

    /// <summary>
    /// 0, 0, -1
    /// </summary>
    public static readonly Vector3D Forward = new(0, 0, -1);

    /// <summary>
    /// 0, 0, 1
    /// </summary>
    public static readonly Vector3D Backward = new(0, 0, 1);

    public static bool operator == (Vector3D a, Vector3D b)
    {
        if (a.GetHashCode() == b.GetHashCode())
            return a.Equals(b);
        return false;
    }

    public static bool operator != (Vector3D a, Vector3D b)
        => !(a == b);
    
    public static bool operator > (Vector3D? a, Vector3D? b)
        => a?.Length > b?.Length;
    
    public static bool operator < (Vector3D? a, Vector3D? b)
        => !(a > b);

    public static bool operator >= (Vector3D? a, Vector3D? b)
        => a > b || a == b;
    
    public static bool operator <= (Vector3D? a, Vector3D? b)
        => a < b || a == b;
    
    public static Vector3D operator + (Vector3D a, Vector3D b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3D operator - (Vector3D a, Vector3D b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector3D operator - (Vector3D a)
        => a * -1;
    
    public static double operator * (Vector3D a, Vector3D b)
        => a.Dot(b);

    public static Vector3D operator * (Vector3D a, double b)
        => new(a.X * b, a.Y * b, a.Z * b);

    public static Vector3D operator * (double a, Vector3D b)
        => b * a;

    public static Vector3D operator / (Vector3D a, double b)
        => new (a.X / b, a.Y / b, a.Z / b);

    public static implicit operator Matrix(Vector3D vector3D)
        => new(new double[,]{{vector3D.X}, {vector3D.Y}, {vector3D.Z}});

    public static explicit operator Vector3D?(Matrix matrix)
    {
        try
        {
            return new(matrix[0,0], matrix[1,0], matrix[2,0]);
        }
        catch(ArgumentOutOfRangeException)
        {
            return null;
        }
    }

    public readonly Vector3D Normalize()
    {
        if (Length is 0)
            return Zero;

        return new Vector3D(X / Length, Y / Length, Z / Length);
    }
        
    public readonly double Dot(Vector3D other)
        =>(((Matrix)this).Transpose() * other)[0, 0];
    
    public readonly Vector3D Cross(Vector3D other)
        => new()
        {
            X = Y * other.Z - Z * other.Y,
            Y = Z * other.X - X * other.Z,
            Z = X * other.Y - Y * other.X
        };
    

    public double Mix(Vector3D otherA, Vector3D otherB)
        => Dot(otherA.Cross(otherB));
    
    public double AngleTo(Vector3D other)
        => Math.Atan2(Cross(other).Length, Dot(other));

    public readonly Vector3D Rotate(double angle, Vector3D axis)
        => (Vector3D)(Transformations.RotationMatrix(angle, axis) * this)!;

    public Vector3D RotateTo(Vector3D other)
        => Rotate(AngleTo(other), Cross(other).Normalize());

    public readonly Vector3D CombineRotation(Vector3D other)
    {
        var rotationX = Transformations.RotationMatrix(other.X, Right);
        var rotationY = Transformations.RotationMatrix(other.Y, Up);
        var rotationZ = Transformations.RotationMatrix(other.Z, Backward);

        return (Vector3D)(rotationX * rotationY * rotationZ * this)!;
    }

    public readonly Vector3D Scale(Vector3D other)
        => (Vector3D)(((Matrix)other).Diagonal() * this)!;

    public readonly Vector3D Shear(Vector3D shearFactor, Vector3D axis)
        => (Vector3D)(Transformations.ShearMatrix(shearFactor, axis) * this)!;

    public readonly Vector3D Lerp(Vector3D to, double weight)
        => new(X.Lerp(to.X, weight), Y.Lerp(to.Y, weight), Z.Lerp(to.Z, weight));

    public readonly double DistanceTo(Vector3D to)
         => (to - this).Length;

    public override readonly string ToString()
        => $"{X}, {Y}, {Z}";

    public override readonly bool Equals(object? obj)
    {
        if (obj != null && obj is Vector3D other && X == other.X && Y == other.Y && Z == other.Z)
            return true;
    
        return false;
    }

    public override readonly int GetHashCode()
        => (X + Y + Z).GetHashCode();
}

