﻿using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class RectangleObject(RectangleData data) : RenderObject<RectangleData>
{
    private static readonly uint[] _indices = 
    [
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    ];

    private Mesh _mesh = new(GetVertices(data.Size.X, data.Size.Y), _indices, data.Material, BufferUsageARB.StaticDraw, PrimitiveType.Triangles);
    private RectangleData _rectangleData = data;

    public override void Draw()
    {
        _mesh.Draw();
    }

    public override void Update(RectangleData data)
    {
        if (data.Size != _rectangleData.Size || data.Material != _rectangleData.Material)
        {
            _mesh.Dispose();
            _mesh = new(GetVertices(data.Size.X, data.Size.Y), _indices, data.Material, BufferUsageARB.StaticDraw, PrimitiveType.Triangles);
        }
        _rectangleData = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        _mesh.Dispose();

        base.Dispose(disposing);
    }

    private static Vertex[] GetVertices(float width, float height)
        => 
        [
            new Vertex  // Bottom left
            {
                Position = new (0.0f, 0.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 0.0f)
            },
            new Vertex  // Top left
            {
                Position = new (0.0f, height, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 1.0f)
            },
            new Vertex // Top right
            {
                Position = new (width, height, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (1.0f, 1.0f)
            },
            new Vertex // Bottom right
            {
                Position = new (width, 0.0f, 0.0f), 
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (1.0f, 0.0f)
            }
        ];
}
