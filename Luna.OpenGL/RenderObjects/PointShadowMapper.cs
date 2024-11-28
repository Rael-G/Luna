using System.Numerics;
using Luna.Maths;
using Silk.NET.OpenGL;

namespace Luna.OpenGL.RenderObjects;

public class PointShadowMapper : ShadowMapper<PointLight>
{
    private const string VertexName = "PointDepthMapVertexShader.glsl";
    private const string GeometryName = "PointDepthMapGeometryShader.glsl";
    private const string FragmentName = "PointDepthMapFragmentShader.glsl";

    private const float NearPlane = 1f;
    private const float FarPlane = 25f;

    private static ShaderSource[] _shaders = 
    [
        new()
        {
            Name = VertexName,
            Path = ProgramShader.DefaultShaderPath(VertexName),
            ShaderType = ShaderType.VertexShader
        },
        new()
        {
            Name = GeometryName,
            Path = ProgramShader.DefaultShaderPath(GeometryName),
            ShaderType = ShaderType.GeometryShader
        },
        new()
        {
            Name = FragmentName,
            Path = ProgramShader.DefaultShaderPath(FragmentName),
            ShaderType = ShaderType.FragmentShader
        }
    ];

    private PointLightShadow _lightShadow;
    private CubeMap _depthMapTexture;
    private Vector2 _resolution;
    private Vector3 _lightPosition;

    public PointShadowMapper(PointLight light)
        : base(new Material(){ Shaders = _shaders })
    {
        _lightShadow = new(light);
        LightEmitter.PointLights.Add(_lightShadow);
        _resolution = new Vector2(1024, 1024);
        CreateDepthMap();
        CreateLightMatrix(light);
    }

    public override void Draw()
    {
        base.Draw();
    }

    public override void Update(PointLight light)
    {
        if (light.Position != _lightPosition)
        {
            CreateLightMatrix(light);
        }
    }

    private void CreateDepthMap()
    {
        FBO.Viewport = _resolution;
        if (TextureManager.Get(_depthMapTexture.Hash) is not null)
        {
            TextureManager.Dispose(_depthMapTexture.Hash);
        }

        _depthMapTexture = new CubeMap()
        {
            Size = _resolution,
            FilterMode = FilterMode.Nearest,
            WrapMode = WrapMode.ClampToBorder,
            Hash = Guid.NewGuid().ToString(),
            ImageType = ImageType.DeathMap
        };

        _lightShadow.ShadowMap = _depthMapTexture;

        var depthMapTexture = TextureManager.Load(_depthMapTexture);
        depthMapTexture.SetBorderColor(Colors.White);

        FBO.AttachTexture(depthMapTexture, FramebufferAttachment.DepthAttachment);
        FBO.Bind();
        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);
        FBO.Unbind();

        FBO.CheckFrameBuffer("PointShadowMapper");
        GlErrorUtils.CheckError("PointShadowMapper CreateDepthMap");
    }

    private void CreateLightMatrix(PointLight light)
    {
        _lightPosition = light.Position;

        var aspect = _resolution.X / _resolution.Y;
        var proj = Matrix4x4.CreatePerspectiveFieldOfView(90f.ToRadians(), aspect, NearPlane, FarPlane);
        Matrix4x4[] shadowMatrix = 
        [
            Matrix4x4.CreateLookAt(light.Position, light.Position + Vector3.UnitX, -Vector3.UnitY) * proj,
            Matrix4x4.CreateLookAt(light.Position, light.Position - Vector3.UnitX, -Vector3.UnitY) * proj,
            Matrix4x4.CreateLookAt(light.Position, light.Position + Vector3.UnitY, Vector3.UnitZ) * proj,
            Matrix4x4.CreateLookAt(light.Position, light.Position - Vector3.UnitY, -Vector3.UnitZ) * proj,
            Matrix4x4.CreateLookAt(light.Position, light.Position + Vector3.UnitZ, -Vector3.UnitY) * proj,
            Matrix4x4.CreateLookAt(light.Position, light.Position - Vector3.UnitZ, -Vector3.UnitY) * proj,
        ];


        Material.Vector3Properties["lightPos"] = light.Position;
        Material.FloatProperties["farPlane"] = FarPlane;
        for (int i = 0; i < shadowMatrix.Length; i++)
        {
            Material.MatricesProperties[$"shadowMatrices[{i}]"] = shadowMatrix[i];
        }

        _lightShadow.FarPlane = FarPlane;
    }

    public override void Dispose(bool disposing)
    {
        LightEmitter.PointLights.Remove(_lightShadow);
        base.Dispose(disposing);
    }
}
