using System.Numerics;
using Luna.Maths;

namespace Luna;

public struct Color
{
    public Color(float r = 0.0f, float g = 0.0f, float b = 0.0f, float a = 1.0f)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public float R 
    { 
        readonly get => _r; 
        set => _r = (float)Math.Clamp(value, 0.0, 1.0);
    }

    public float G 
    { 
        readonly get => _g;
        set => _g = (float)Math.Clamp(value, 0.0, 1.0);
    }

    public float B 
    { 
        readonly get => _b;
        set => _b = (float)Math.Clamp(value, 0.0, 1.0); 
    }

    public float A 
    { 
        readonly get => _a;
        set => _a = (float)Math.Clamp(value, 0.0, 1.0);
    }

    private float _r;
    private float _g;
    private float _b;
    private float _a;

    public static implicit operator System.Drawing.Color (Color color)
        => System.Drawing.Color.FromArgb(
            (int)0.0f.Lerp(255.0f, color.A), 
            (int)0.0f.Lerp(255.0f, color.R), 
            (int)0.0f.Lerp(255.0f, color.G), 
            (int)0.0f.Lerp(255.0f, color.B)
        );

    public static implicit operator Color(System.Drawing.Color color)
        => new()
    {
        R = color.R / 255.0f,
        G = color.G / 255.0f,
        B = color.B / 255.0f,
        A = color.A / 255.0f
    };

    public readonly Vector3 RGB()
        => new(R, G, B);

    public readonly float[] ToFloatArray()
        => [R, G, B, A];

    // public readonly Color Lerp(Color other, float weight)
    //     => (Color)ToColor(ToMatrix() + (other.ToMatrix() - ToMatrix()) * weight)!;

    // public readonly Color Mix(Color color)
    //     => Lerp(color, 0.5f);

    public static Color Desaturate(Color c)
    {
        var gray = (c.R + c.G + c.B) / 3.0f;
        return new(gray, gray, gray, c.A);
    }

    public static Color Invert(Color c)
        => new(1.0f - c.R, 1.0f - c.G, 1.0f - c.B, c.A);
}
