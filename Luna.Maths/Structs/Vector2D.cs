namespace Luna.Maths;

public struct Vector2D(double x, double y)
{
    public double X { get; set; } = x;

    public double Y { get; set; } = y;

    public readonly double Length 
    { 
        get => Math.Sqrt( X * X + Y * Y);
    }

    public static readonly Vector2D Zero = new(0, 0);
    public static readonly Vector2D Right = new(1, 0);
    public static readonly Vector2D Left = new(-1, 0);
    public static readonly Vector2D Up = new(0, 1);
    public static readonly Vector2D Down = new(0, -1);

    public static bool operator == (Vector2D a, Vector2D b)
    {
        if (a.GetHashCode() == b.GetHashCode())
            return a.Equals(b);
        return false;
    }

    public static bool operator != (Vector2D a, Vector2D b)
        => !(a == b);
    
    public static bool operator > (Vector2D? a, Vector2D? b)
        => a?.Length > b?.Length;
    
    public static bool operator < (Vector2D? a, Vector2D? b)
        => !(a > b);

    public static bool operator >= (Vector2D? a, Vector2D? b)
        => a > b || a == b;
    
    public static bool operator <= (Vector2D? a, Vector2D? b)
        => a < b || a == b;

    public static Vector2D operator + (Vector2D a, Vector2D b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Vector2D operator - (Vector2D a, Vector2D b)
        => new(a.X - b.X, a.Y - b.Y);

    public static Vector2D operator - (Vector2D a)
        => a * -1;
    
    public static double operator * (Vector2D a, Vector2D b)
        => a.Dot(b);

    public static Vector2D operator * (Vector2D a, double b)
        => new(a.X * b, a.Y * b);

    public static Vector2D operator * (double a, Vector2D b)
        => b * a;

    public static Vector2D operator / (Vector2D a, double b)
        => new (a.X / b, a.Y / b);
    
    public static implicit operator Vector3D(Vector2D vector2)
        => new(vector2.X, vector2.Y, 0.0);

    public static implicit operator Matrix(Vector2D vector2D)
        => new(new double[,]{{vector2D.X}, {vector2D.Y}, {0.0}});

    public static explicit operator Vector2D?(Matrix matrix)
    {
        try
        {
            return new(matrix[0,0], matrix[1,0]);
        }
        catch(ArgumentOutOfRangeException)
        {
            return null;
        }
    }

    public Vector2D Normalize()
    {
        if (Length is 0)
            return Zero;

        return new Vector2D(X / Length, Y / Length);
    }

    public readonly double Dot(Vector2D other)
        =>(((Matrix)this).Transpose() * other)[0, 0];
    
    public readonly double Cross(Vector2D other)
        => X * other.Y - Y * other.X;
    
    public readonly double AngleTo(Vector2D other)
    {
        var cosTheta = Dot(other) / (Length * other.Length);
        var angle = Math.Acos(cosTheta);
        return Cross(other) > 0? angle : -angle;
    }
    
    public readonly Vector2D Rotate(double angle)
        => (Vector2D)(Transformations.RotationMatrix(angle, Vector3D.Backward) * this)!;
    
    public Vector2D RotateTo(Vector2D other)
        => Rotate(AngleTo(other));

    public readonly Vector2D Scale(Vector2D other)
        => (Vector2D)(((Matrix)other).Diagonal() * this)!;

    public readonly Vector2D Shear(Vector2D shearFactor)
        => (Vector2D)(Transformations.ShearMatrix(shearFactor, Vector3D.Backward) * this)!;
    
    public readonly Vector2D Lerp(Vector2D to, double weight)
        => new(X.Lerp(to.X, weight), Y.Lerp(to.Y, weight));

    public readonly double DistanceTo(Vector2D to)
        => (to - this).Length;

    public override readonly string ToString()
        => $"X:{X}, Y:{Y}";

    public override readonly bool Equals(object? obj)
    {
        if (obj != null && obj is Vector2D other && X == other.X && Y == other.Y)
            return true;
    
        return false;
    }

    public override readonly int GetHashCode()
        => (X + Y).GetHashCode();

}