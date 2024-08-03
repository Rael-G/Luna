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
            // Vertices                            // Normals           //Texture Coords
            0.0f,           0.0f,           0.0f,   0.0f, 0.0f, 1.0f,    0.0f, 0.0f, // Bottom left
            0.0f,           data.Size.Y,    0.0f,   0.0f, 0.0f, 1.0f,    0.0f, 1.0f, // Top left
            data.Size.X,    data.Size.Y,    0.0f,   0.0f, 0.0f, 1.0f,    1.0f, 1.0f, //Top right
            data.Size.X,    0.0f,           0.0f,   0.0f, 0.0f, 1.0f,    1.0f, 0.0f, // Bottom right
        ];

        data.Material.Color = data.Color;
        data.Material.ModelViewProjection = data.ModelViewProjection;

        return new PolygonData()
        {
            Vertices = vertices,
            Indices = _indices,
            PrimitiveType = PrimitiveType.Triangles,
            BufferUsage = BufferUsageARB.StaticDraw,
            VerticeInfo = new()
            {
                Size = 3,
                Lengths = [3, 3, 2],
            },
            Material = data.Material
        };
    }

    
}
