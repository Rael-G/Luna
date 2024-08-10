using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class EllipseObject(EllipseData data) : RenderObject<EllipseData>
{
    private Mesh _mesh = new (GenerateVertices(data.Radius, data.Segments), GenerateIndices(data.Segments), data.Material, BufferUsageARB.StaticDraw, PrimitiveType.TriangleFan);
    private EllipseData _ellipseData = data;

    public override void Draw()
        => _mesh.Draw();
    

    public override void Update(EllipseData data)
    {
        if (data.Radius != _ellipseData.Radius || data.Segments != _ellipseData.Segments || data.Material != _ellipseData.Material)
        {
            _mesh.Dispose();
            _mesh = new Mesh(GenerateVertices(data.Radius, data.Segments), GenerateIndices(data.Segments), data.Material, BufferUsageARB.StaticDraw, PrimitiveType.TriangleFan);
        }
        _ellipseData = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        _mesh.Dispose();

        base.Dispose(disposing);
    }

    private static Vertex[] GenerateVertices(Vector2 radius, int segments)
    {
        List<Vertex> vertices =
        [
            new Vertex
            {
                Position = new Vector3(0.0f, 0.0f, 0.0f),
                Normal = new Vector3(0.0f, 0.0f, 1.0f),
                TexCoords = new Vector2(0.5f, 0.5f)
            }
        ];

        float angleStep = 2.0f * MathF.PI / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep;
            float x = radius.X * MathF.Cos(angle);
            float y = radius.Y * MathF.Sin(angle);

            vertices.Add(new Vertex
            {
                Position = new Vector3(x, y, 0.0f),
                Normal = new Vector3(0.0f, 0.0f, 1.0f),
                TexCoords = new Vector2(x / radius.X * 0.5f + 0.5f,
                                        y / radius.Y * 0.5f + 0.5f)
            });
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

