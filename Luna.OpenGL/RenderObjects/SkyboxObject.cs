using FreeTypeSharp;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class SkyboxObject : RenderObject<SkyboxData>
{
    private readonly Mesh _mesh = new(_vertices, _indices);
    private SkyboxData _data;

    private const string VertexName = "SkyboxVertexShader.glsl";
    private const string FragmentName = "SkyboxFragmentShader.glsl";

    private readonly Material _material = new()
    {
        Shaders = 
        [
            new()
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
            }
        ]
    };


    public SkyboxObject(SkyboxData data)
    {
        _data = data;
        _material.SetCubeMap("skybox", data.CubeMap);
        Priority = -1;
    }

    public override void Draw()
    {
        SetMVP(_material, _data.ModelViewProjection);
        var window = Injector.Get<IWindow>();
        var previousDepthMode = window.DepthMode;

        window.DepthMode = DepthMode.Lequal;
        _material.Bind();
        _mesh.Draw(PrimitiveType.Triangles);
        _material.Unbind();
        window.DepthMode = previousDepthMode;
        GlErrorUtils.CheckError("SkyboxDraw");
    }

    public override void Draw(IMaterial material)
    {
        // Do nothing
    }

    public override void Update(SkyboxData data)
    {
        _data = data;
        _material.MatricesProperties["view"] = _data.ModelViewProjection.View;
        _material.MatricesProperties["projection"] = _data.ModelViewProjection.Projection;
    }

    private static readonly uint[] _indices =
    [
        // Front Face
        4, 7, 6, 6, 5, 4,
        // Back Face
        0, 1, 2, 2, 3, 0,
        // Left Face
         0, 4, 5, 5, 1, 0,
        // Right Face
        3, 2, 6, 6, 7, 3,
        // Top Face
        0, 3, 7, 7, 4, 0,
        // Bottom Face
        1, 5, 6, 6, 2, 1
    ];

    private static readonly Vertex[] _vertices
        =
        [
            new Vertex() { Position = new(-1.0f,  1.0f, -1.0f) },
            new Vertex() { Position = new(-1.0f, -1.0f, -1.0f) },
            new Vertex() { Position = new( 1.0f, -1.0f, -1.0f) },
            new Vertex() { Position = new( 1.0f,  1.0f, -1.0f) },
            new Vertex() { Position = new(-1.0f,  1.0f,  1.0f) },
            new Vertex() { Position = new(-1.0f, -1.0f,  1.0f) },
            new Vertex() { Position = new( 1.0f, -1.0f,  1.0f) },
            new Vertex() { Position = new( 1.0f,  1.0f,  1.0f) },
        ];
}
