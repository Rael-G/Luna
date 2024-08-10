
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class BoxObject(BoxData data) : RenderObject<BoxData>
{
    private static readonly uint[] _indices =
    [
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
    ];

    private Mesh _mesh = new(GetVertices(data.Size.X, data.Size.Y, data.Size.Z), _indices, data.Material, BufferUsageARB.StaticDraw, PrimitiveType.Triangles);
    private BoxData _boxData = data;

    public override void Draw()
        => _mesh.Draw();
    
    public override void Update(BoxData data)
    {
        if (data.Size != _boxData.Size || data.Material != _boxData.Material)
        {
            _mesh.Dispose();
            _mesh = new(GetVertices(data.Size.X, data.Size.Y, data.Size.Z), _indices, data.Material, BufferUsageARB.StaticDraw, PrimitiveType.Triangles);
        }
        _boxData = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        _mesh.Dispose();

        base.Dispose(disposing);
    }

    private static Vertex[] GetVertices(float width, float height, float depth)
        =>
        [
            // Front face
            new Vertex()
            {
                Position = new (0.0f, 0.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 0.0f)
            }, // Bottom left

            new Vertex()
            {
                Position = new (width, 0.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (1.0f, 0.0f)
            }, // Bottom right

            new Vertex()
            {
                Position = new (width, height, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (1.0f, 1.0f)
            }, // Top right

            new Vertex()
            {
                Position = new (0.0f, height, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 1.0f)
            }, // Top left

            // Back face
            new Vertex()
            {
                Position = new (0.0f, 0.0f, -depth),
                Normal = new (0.0f, 0.0f, -1.0f),
                TexCoords = new (0.0f, 0.0f)
            }, // Bottom left

            new Vertex()
            {
                Position = new (width, 0.0f, -depth),
                Normal = new (0.0f, 0.0f, -1.0f),
                TexCoords = new (1.0f, 0.0f)
            }, // Bottom right

            new Vertex()
            {
                Position = new (width, height, -depth),
                Normal = new (0.0f, 0.0f, -1.0f),
                TexCoords = new (1.0f, 1.0f)
            }, // Top right

            new Vertex()
            {
                Position = new (0.0f, height, -depth),
                Normal = new (0.0f, 0.0f, -1.0f),
                TexCoords = new (0.0f, 1.0f)
            }, // Top left

            // Left Face
            new Vertex()
            {
                Position = new (0.0f, 0.0f, -depth),
                Normal = new (-1.0f, 0.0f, 0.0f),
                TexCoords = new (0.0f, 0.0f)
            }, // Bottom left

            new Vertex()
            {
                Position = new (0.0f, 0.0f, 0.0f),
                Normal = new (-1.0f, 0.0f, 0.0f),
                TexCoords = new (1.0f, 0.0f)
            }, // Bottom right

            new Vertex()
            {
                Position = new (0.0f, height, 0.0f),
                Normal = new (-1.0f, 0.0f, 0.0f),
                TexCoords = new (1.0f, 1.0f)
            }, // Top right

            new Vertex()
            {
                Position = new (0.0f, height, -depth),
                Normal = new (-1.0f, 0.0f, 0.0f),
                TexCoords = new (0.0f, 1.0f)
            }, // Top left

            // Right face
            new Vertex()
            {
                Position = new (width, 0.0f, 0.0f),
                Normal = new (1.0f, 0.0f, 0.0f),
                TexCoords = new (0.0f, 0.0f)
            }, // Bottom left

            new Vertex()
            {
                Position = new (width, 0.0f, -depth),
                Normal = new (1.0f, 0.0f, 0.0f),
                TexCoords = new (1.0f, 0.0f)
            }, // Bottom right

            new Vertex()
            {
                Position = new (width, height, -depth),
                Normal = new (1.0f, 0.0f, 0.0f),
                TexCoords = new (1.0f, 1.0f)
            }, // Top right

            new Vertex()
            {
                Position = new (width, height, 0.0f),
                Normal = new (1.0f, 0.0f, 0.0f),
                TexCoords = new (0.0f, 1.0f)
            }, // Top left

            // Bottom face
            new Vertex()
            {
                Position = new (0.0f, 0.0f, -depth),
                Normal = new (0.0f, -1.0f, 0.0f),
                TexCoords = new (0.0f, 0.0f)
            }, // Bottom left

            new Vertex()
            {
                Position = new (width, 0.0f, -depth),
                Normal = new (0.0f, -1.0f, 0.0f),
                TexCoords = new (1.0f, 0.0f)
            }, // Bottom right

            new Vertex()
            {
                Position = new (width, 0.0f, 0.0f),
                Normal = new (0.0f, -1.0f, 0.0f),
                TexCoords = new (1.0f, 1.0f)
            }, // Top right

            new Vertex()
            {
                Position = new (0.0f, 0.0f, 0.0f),
                Normal = new (0.0f, -1.0f, 0.0f),
                TexCoords = new (0.0f, 1.0f)
            }, // Top left

            // Top face
            new Vertex()
            {
                Position = new (0.0f, height, 0.0f),
                Normal = new (0.0f, 1.0f, 0.0f),
                TexCoords = new (0.0f, 0.0f)
            }, // Bottom left

            new Vertex()
            {
                Position = new (width, height, 0.0f),
                Normal = new (0.0f, 1.0f, 0.0f),
                TexCoords = new (1.0f, 0.0f)
            }, // Bottom right

            new Vertex()
            {
                Position = new (width, height, -depth),
                Normal = new (0.0f, 1.0f, 0.0f),
                TexCoords = new (1.0f, 1.0f)
            }, // Top right

            new Vertex()
            {
                Position = new (0.0f, height, -depth),
                Normal = new (0.0f, 1.0f, 0.0f),
                TexCoords = new (0.0f, 1.0f)
            } // Top left
        ];
}
