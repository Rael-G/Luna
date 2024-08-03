using System.Numerics;

namespace Luna;

public struct EllipseData
{
    public Vector2 Radius { get; set; }
    public int Segments { get; set; }
    public Color Color { get; set; }
    public ModelViewProjection ModelViewProjection { get; set; }
    public IStandardMaterial Material { get; set; }
}
