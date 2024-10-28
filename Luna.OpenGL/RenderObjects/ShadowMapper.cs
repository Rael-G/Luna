using System.Numerics;

namespace Luna.OpenGL.RenderObjects;

public class ShadowMapper : RenderObject<DirectionalLight>
{
    private const string VertexName = "DepthMapVertexShader.glsl";
    private const string FragmentName = "DepthMapFragmentShader.glsl";

    private static ShaderSource[] _shader = 
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
    ];

    private DepthMap _depthMap;
    private Matrix4x4 _lightSpaceMatrix;

    private Material _material = new()
    {
        Shaders = _shader
    };

    private Vector3 _lightPosition;
    private Vector3 _lightDirection;

    public ShadowMapper(DirectionalLight light)
    {
        CreateLightMatrix(light);
        _depthMap = new(new Vector2(1024, 1024), GL);
        Priority = 1;
    }

    public override void Draw()
    {
        _depthMap.Bind();
        Injector.Get<IRenderer>().DrawQueue(_material, false);
        _depthMap.Unbind();

        LightEmitter.ShadowMaps.Add((_lightSpaceMatrix, _depthMap.DepthMapTexture));
    }

    public override void Draw(IMaterial material)
    {
        // Do nothing
    }

    public override void Update(DirectionalLight light)
    {
        if (light.Position != _lightPosition || light.Direction != _lightDirection)
        {
            CreateLightMatrix(light);
        }
    }

    private void CreateLightMatrix(DirectionalLight light)
    {
        _lightPosition = light.Position;
        _lightDirection = light.Direction;
        var lightProjection = Matrix4x4.CreateOrthographic(20, 20, 1f, 50);
        Vector3 up = Vector3.Cross(light.Direction, Vector3.UnitY).LengthSquared() < 0.001f
            ? Vector3.UnitZ
            : Vector3.UnitY;
        var lightView = Matrix4x4.CreateLookAt(light.Position, light.Position + light.Direction * 1000, up);
        _lightSpaceMatrix = lightView * lightProjection;
        _material.MatricesProperties["lightSpaceMatrix"] = _lightSpaceMatrix;
    }
}