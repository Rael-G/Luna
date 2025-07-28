using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class BoxObject(BoxData data) : RenderObject<BoxData>
{
    private static readonly uint[] _indices =
    [
        // Front face
        0, 1, 2, 2, 3, 0,
        // Back face
        4, 6, 5, 6, 4, 7,
        // Left Face
        10, 11, 8, 8, 9, 10,
        // Right face
        15, 13, 14, 13, 15, 12,
        // Bottom face
        18, 19, 16, 16, 17, 18,
        // Top face
        23, 21, 22, 21, 23, 20,
    ];

    private Mesh _mesh = new(
        Tangent.CalculateTangents(
            GetVertices(data.Size.X, data.Size.Y, data.Size.Z),
            _indices),
        _indices
    );

    private BoxData _boxData = data;

    public override void Draw()
    {
        Draw(_boxData.Material);
    }

    public override void Draw(IMaterial material)
    {
        SetMVP(material, _boxData.ModelViewProjection);
        material.Bind();
        _mesh.Draw(PrimitiveType.Triangles);
        material.Unbind();
    }
    
    public override void Update(BoxData data)
    {
        if (data.Size != _boxData.Size || data.Material != _boxData.Material)
        {
            _mesh.Dispose();
            _mesh = new(
                Tangent.CalculateTangents(
                    GetVertices(data.Size.X, data.Size.Y, data.Size.Z),
                    _indices),
                _indices
            );
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
