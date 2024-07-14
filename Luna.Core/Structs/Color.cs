using System.Collections;
using Luna.Maths;

namespace Luna.Core;

public readonly struct Color : IEnumerable<double>
{
    public Color(double r = 0.0, double g = 0.0, double b = 0.0, double a = 0.0)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public readonly double R 
    { 
        get => Matrix[0, 0]; 
        set => Matrix[0, 0] = Math.Clamp(value, 0.0, 1.0);
    }

    public readonly double G 
    { 
        get =>  Matrix[1, 0];
        set => Matrix[1, 0] = Math.Clamp(value, 0.0, 1.0);
    }

    public readonly double B 
    {
        get =>  Matrix[2, 0];
        set => Matrix[2, 0] = Math.Clamp(value, 0.0, 1.0); 
    }

    public readonly double A 
    { 
        get =>  Matrix[3, 0];
        set => Matrix[3, 0] = Math.Clamp(value, 0.0, 1.0);
    }

    private readonly Matrix Matrix = new(4, 1);

    public static implicit operator Matrix(Color color)
    => new(color.Matrix);
    
    public static explicit operator Color(Matrix matrix)
        => new(matrix[0,0], matrix[1,0], matrix[2,0], matrix[3,0]);

    public Color Lerp(Color color, double weight)
        => (Color)Matrix.Lerp(color.Matrix, weight);

    public Color Mix(Color color)
        => Lerp(color, 0.5);

    public static Color Desaturate(Color c)
    {
        var gray = (c.R + c.G + c.B) / 3.0f;
        return new(gray, gray, gray, c.A);
    }

    public static Color Invert(Color c)
        => new(1.0f - c.R, 1.0f - c.G, 1.0f - c.B, c.A);

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public IEnumerator<double> GetEnumerator()
        => Matrix.GetEnumerator();

}
