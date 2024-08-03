using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public struct PolygonData
{
    public float[] Vertices { get; set; }
    public uint[] Indices { get; set; }
    public VerticesInfo VerticeInfo { get; set; }
    public Color Color { get; set; }
    public PrimitiveType PrimitiveType { get; set; }
    public BufferUsageARB BufferUsage;
    public IMaterial Material;
}
