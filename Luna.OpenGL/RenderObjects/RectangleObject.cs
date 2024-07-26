using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class RectangleObject(RectangleData data) : PolygonObject<RectangleData>(ToPolygonData(data))
{
    private static readonly uint[] _indices = 
    [
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    ];

    public override void Update(RectangleData data)
    {
        Update(ToPolygonData(data));
    }

    private static PolygonData ToPolygonData(RectangleData data)
    {
        float[] vertices =
        [
            0.0f, 0.0f, 0.0f,  // Bottom left
            0.0f, data.Size.Y, 0.0f,  // Top left
            data.Size.X, data.Size.Y, 0.0f,   //Top right
            data.Size.X, 0.0f, 0.0f,    // Bottom right
        ];

        return new PolygonData()
        {
            Vertices = vertices,
            Indices = _indices,
            Color = data.Color,
            Transform = data.Transform,
            PrimitiveType = PrimitiveType.Triangles
        };
    }

    
}
