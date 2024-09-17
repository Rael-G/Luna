using Silk.NET.OpenGL;

namespace Luna.OpenGL.RenderObjects;

public class SkyboxObject : RenderObject<SkyboxData>
{
    private readonly Mesh _mesh = new(_vertices, _indices);
    private SkyboxData _data;

    private const string VertexName = "SkyboxVertexShader.glsl";
    private const string FragmentName = "SkyboxFragmentShader.glsl";

    private readonly Material _material = new(
        [new()
        {
            Name = VertexName,
            Path = ProgramShader.DefaultShaderPath(VertexName),
            ShaderType = ShaderType.VertexShader
        },
        new()
        {
            Name = FragmentName,
            Path = ProgramShader.DefaultShaderPath(FragmentName),
            ShaderType = ShaderType.FragmentShader
        }]
    );

    public SkyboxObject(SkyboxData data)
    {
        _data = data;
        _material.MatricesProperties["view"] = _data.ModelViewProjection.View;
        _material.MatricesProperties["projection"] = _data.ModelViewProjection.Projection;
        _material.SetTexture("skybox", GlCubeMap.Load(data.CubeMap));
    }

    public override void Draw()
    {
        GL.DepthFunc(DepthFunction.Lequal);
        _material.Bind();
        _mesh.Draw(PrimitiveType.Triangles);
        GL.DepthMask(true);
        GL.DepthFunc(DepthFunction.Less);

    }

    public override void Update(SkyboxData data)
    {
        _data = data;
        _material.MatricesProperties["view"] = _data.ModelViewProjection.View;
        _material.MatricesProperties["projection"] = _data.ModelViewProjection.Projection;
    }

    private static readonly uint[] _indices =
    [
        // Face da frente
        4, 7, 6, 6, 5, 4,
        // Face de trás
        0, 1, 2, 2, 3, 0,
        // Face esquerda
         0, 4, 5, 5, 1, 0,
        // Face direita
        3, 2, 6, 6, 7, 3,
        // Face superior
        0, 3, 7, 7, 4, 0,
        // Face inferior
        1, 5, 6, 6, 2, 1
    ];

    private static readonly Vertex[] _vertices
        =
        [
            new Vertex() { Position = new(-1.0f,  1.0f, -1.0f) }, // 0
            new Vertex() { Position = new(-1.0f, -1.0f, -1.0f) }, // 1
            new Vertex() { Position = new( 1.0f, -1.0f, -1.0f) }, // 2
            new Vertex() { Position = new( 1.0f,  1.0f, -1.0f) }, // 3
            new Vertex() { Position = new(-1.0f,  1.0f,  1.0f) }, // 4
            new Vertex() { Position = new(-1.0f, -1.0f,  1.0f) }, // 5
            new Vertex() { Position = new( 1.0f, -1.0f,  1.0f) }, // 6
            new Vertex() { Position = new( 1.0f,  1.0f,  1.0f) }, // 7
        ];
}
