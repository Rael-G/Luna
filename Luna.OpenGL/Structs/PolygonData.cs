using Luna.Maths;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public struct PolygonData
{
    public float[] Vertices { get; set; }
    public uint[] Indices { get; set; }
    public Matrix Transform { get; set; }
    public Color Color { get; set; }
    public Shader[]? Shader { get; set; }
    public PrimitiveType PrimitiveType { get; set; }
}
