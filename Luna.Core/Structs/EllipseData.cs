using System.Numerics;
using Luna.Maths;

namespace Luna;

public struct EllipseData
{
    public Vector2 Radius { get; set; }
    public int Segments { get; set; }
    public Color Color { get; set; }
    public Matrix Transform { get; set; }
}
