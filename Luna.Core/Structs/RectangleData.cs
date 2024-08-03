using System.Numerics;

namespace Luna;

public struct RectangleData
{
    public Vector2 Size { get; set; }
    public Color Color { get; set; }
    public ModelViewProjection ModelViewProjection { get; set; }
    public IStandardMaterial Material { get; set; }
}
