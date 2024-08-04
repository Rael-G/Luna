
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class BoxObject(BoxData boxData) : PolygonObject<BoxData>(ToPolygonData(boxData))
{
    private static readonly uint[] _indices =
    {
        // Front face
        0, 1, 2, 0, 2, 3,
        // Back face
        4, 5, 6, 4, 6, 7,
        // Left Face
        8, 9, 10, 8, 10, 11,
        // Right face
        12, 13, 14, 12, 14, 15,
        // Bottom face
        16, 17, 18, 16, 18, 19,
        // Top face
        20, 21, 22, 20, 22, 23,
    };

    public override void Update(BoxData data)
    {
        Update(ToPolygonData(data));
    }

    private static PolygonData ToPolygonData(BoxData data)
    {
        float width = data.Size.X;
        float height = data.Size.Y;
        float depth = data.Size.Z;

        float[] vertices =
        {
            // Positions             // Normals          // Texture Coords
            // Front face
            0.0f, 0.0f, 0.0f,        0.0f, 0.0f, 1.0f,   0.0f, 0.0f, // Bottom left
            width, 0.0f, 0.0f,       0.0f, 0.0f, 1.0f,   1.0f, 0.0f, // Bottom right
            width, height, 0.0f,     0.0f, 0.0f, 1.0f,   1.0f, 1.0f, // Top right
            0.0f, height, 0.0f,      0.0f, 0.0f, 1.0f,   0.0f, 1.0f, // Top left

            // Back face
            0.0f, 0.0f, -depth,      0.0f, 0.0f, -1.0f,  0.0f, 0.0f, // Bottom left
            width, 0.0f, -depth,     0.0f, 0.0f, -1.0f,  1.0f, 0.0f, // Bottom right
            width, height, -depth,   0.0f, 0.0f, -1.0f,  1.0f, 1.0f, // Top right
            0.0f, height, -depth,    0.0f, 0.0f, -1.0f,  0.0f, 1.0f, // Top left

            // Left Face
            0.0f, 0.0f, -depth,      -1.0f, 0.0f, 0.0f,  0.0f, 0.0f, // Bottom left
            0.0f, 0.0f, 0.0f,        -1.0f, 0.0f, 0.0f,  1.0f, 0.0f, // Bottom right
            0.0f, height, 0.0f,      -1.0f, 0.0f, 0.0f,  1.0f, 1.0f, // Top right
            0.0f, height, -depth,    -1.0f, 0.0f, 0.0f,  0.0f, 1.0f, // Top left

            // Right face
            width, 0.0f, 0.0f,       1.0f, 0.0f, 0.0f,   0.0f, 0.0f, // Bottom left
            width, 0.0f, -depth,     1.0f, 0.0f, 0.0f,   1.0f, 0.0f, // Bottom right
            width, height, -depth,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, // Top right
            width, height, 0.0f,     1.0f, 0.0f, 0.0f,   0.0f, 1.0f, // Top left

            // Bottom face
            0.0f, 0.0f, -depth,      0.0f, -1.0f, 0.0f,  0.0f, 0.0f, // Bottom left
            width, 0.0f, -depth,     0.0f, -1.0f, 0.0f,  1.0f, 0.0f, // Bottom right
            width, 0.0f, 0.0f,       0.0f, -1.0f, 0.0f,  1.0f, 1.0f, // Top right
            0.0f, 0.0f, 0.0f,        0.0f, -1.0f, 0.0f,  0.0f, 1.0f, // Top left

            // Top face
            0.0f, height, 0.0f,      0.0f, 1.0f, 0.0f,   0.0f, 0.0f, // Bottom left
            width, height, 0.0f,     0.0f, 1.0f, 0.0f,   1.0f, 0.0f, // Bottom right
            width, height, -depth,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f, // Top right
            0.0f, height, -depth,    0.0f, 1.0f, 0.0f,   0.0f, 1.0f, // Top left
        };

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
