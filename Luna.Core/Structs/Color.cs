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

    //values from 0 - 255
    public Color(int r = 0, int g = 0, int b = 0, int a = 255)
    {
        R = Math.Clamp(r, 0, 255) / 255.0f;
        G = Math.Clamp(g, 0, 255) / 255.0f;
        B = Math.Clamp(b, 0, 255) / 255.0f;
        A = Math.Clamp(a, 0, 255) / 255.0f;
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
        => new(color.R, color.G, color.B, color.A);

    public readonly Vector3 RGB()
        => new(R, G, B);

    public readonly float[] ToFloatArray()
        => [R, G, B, A];

    public readonly Color Lerp(Color other, float weight)
        => new (R.Lerp(other.R, weight), G.Lerp(other.G, weight), B.Lerp(other.B, weight), A.Lerp(other.A, weight));

    public readonly Color Mix(Color color)
        => Lerp(color, 0.5f);

    public static Color AdjustHue(Color c, float hue)
    {
        var (h, s, v) = ToHSV(c);
        h = (h + hue) % 360.0f;
        return FromHSV(h, s, v, c.A);
    }

    public static Color AdjustSaturation(Color c, float saturation)
    {
        var (h, s, v) = ToHSV(c);
        s = Math.Clamp(s * saturation, 0.0f, 1.0f);
        return FromHSV(h, s, v, c.A);
    }

    public static Color AdjustBrightness(Color c, float brightness)
    {
        var (h, s, v) = ToHSV(c);
        v = Math.Clamp(v * brightness, 0.0f, 1.0f);
        return FromHSV(h, s, v, c.A);
    }

    public static Color AdjustContrast(Color c, float contrast)
    {
        contrast = Math.Clamp(contrast, -1.0f, 1.0f);
        float factor = 259 * (contrast + 1) / (255 * (259 - contrast));

        float Adjust(float channel)
        {
            return Math.Clamp(factor * (channel - 0.5f) + 0.5f, 0.0f, 1.0f);
        }

        return new Color(Adjust(c.R), Adjust(c.G), Adjust(c.B), c.A);
    }


    private static (float H, float S, float V) ToHSV(Color c)
    {
        float max = Math.Max(c.R, Math.Max(c.G, c.B));
        float min = Math.Min(c.R, Math.Min(c.G, c.B));
        float delta = max - min;

        float h = 0.0f;
        if (delta > 0)
        {
            if (max == c.R)
                h = (c.G - c.B) / delta;
            else if (max == c.G)
                h = 2.0f + (c.B - c.R) / delta;
            else
                h = 4.0f + (c.R - c.G) / delta;

            h *= 60;
            if (h < 0)
                h += 360;
        }

        float s = (max == 0) ? 0 : (delta / max);
        float v = max;

        return (h, s, v);
    }

    private static Color FromHSV(float h, float s, float v, float a = 1.0f)
    {
        h = h % 360;
        if (h < 0) h += 360;
        float c = v * s;
        float x = c * (1 - Math.Abs((h / 60) % 2 - 1));
        float m = v - c;

        float r1 = 0, g1 = 0, b1 = 0;
        if (h < 60) { r1 = c; g1 = x; b1 = 0; }
        else if (h < 120) { r1 = x; g1 = c; b1 = 0; }
        else if (h < 180) { r1 = 0; g1 = c; b1 = x; }
        else if (h < 240) { r1 = 0; g1 = x; b1 = c; }
        else if (h < 300) { r1 = x; g1 = 0; b1 = c; }
        else { r1 = c; g1 = 0; b1 = x; }

        return new Color(r1 + m, g1 + m, b1 + m, a);
    }
}
