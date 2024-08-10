using System.Numerics;

namespace Luna;

public struct EllipseData
{
    public Vector2 Radius { get; set; }
    public int Segments { get; set; }
    public IStandardMaterial Material { get; set; }
}
