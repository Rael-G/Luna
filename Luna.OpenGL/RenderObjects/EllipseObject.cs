using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class EllipseObject(EllipseData data) : PolygonObject<EllipseData>(ToPolygonData(data))
{
    public override void Update(EllipseData data)
    {
        Update(ToPolygonData(data));
    }

    private static PolygonData ToPolygonData(EllipseData data)
    {
        return new()
        {
            Vertices = GenerateVertices(data.Radius, data.Segments),
            Indices = GenerateIndices(data.Segments),
            PrimitiveType = PrimitiveType.TriangleFan,
            BufferUsage = BufferUsageARB.StaticDraw,
            VerticeInfo = new()
            {
                Size = 3,
                Lengths = [3, 3, 2],
            },
            Material = data.Material
        };
    }

    private static float[] GenerateVertices(Vector2 radius, int segments)
    {
        List<float> vertices = 
        [
            0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f,
            0.5f, 0.5f
        ];

        float angleStep = 2.0f * MathF.PI / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep;
            float x = radius.X * MathF.Cos(angle);
            float y = radius.Y * MathF.Sin(angle);

            vertices.Add(x, y, 0.0f);
            vertices.Add(0.0f, 0.0f, 1.0f);
            vertices.Add(x / radius.X * 0.5f + 0.5f, y / radius.Y * 0.5f + 0.5f);
        }

        return [.. vertices];
    }

    private static uint[] GenerateIndices(int segments)
    {
        List<uint> indices = [0];

        for (uint i = 1; i <= segments; i++)
        {
            indices.Add(i);
        }

        indices.Add(1);

        return [.. indices];
    }

}
