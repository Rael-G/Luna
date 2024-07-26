using System.Numerics;
using Luna.Core;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class EllipseObject(EllipseData data) : PolygonObject<EllipseData>(ToPolygonData(data))
{
    public override void Update(EllipseData data)
    {
        Update(ToPolygonData(data));
    }

    private static PolygonData ToPolygonData(EllipseData data)
        => new()
        {
            Vertices = GenerateVertices(data.Radius, data.Segments),
            Indices = GenerateIndices(data.Segments),
            Color = data.Color,
            Transform = data.Transform,
            PrimitiveType = PrimitiveType.TriangleFan
        };

    private static float[] GenerateVertices(Vector2 radius, int segments)
    {
        List<float> vertices = [0.0f, 0.0f, 0.0f];

        float angleStep = 2.0f * MathF.PI / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep;
            float x = radius.X * MathF.Cos(angle);
            float y = radius.Y * MathF.Sin(angle);

            vertices.Add(x);
            vertices.Add(y);
            vertices.Add(0.0f);
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
