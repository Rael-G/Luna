using System;
using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL.RenderObjects;

public class DirectionalShadowMapper : ShadowMapper<DirectionalLight>
{
    private const string VertexName = "DepthMapVertexShader.glsl";
    private const string FragmentName = "DepthMapFragmentShader.glsl";

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
            Name = FragmentName,
            Path = ProgramShader.DefaultShaderPath(FragmentName),
            ShaderType = ShaderType.FragmentShader
        }
    ];

    public Texture2D _depthMapTexture;
    private Matrix4x4 _lightSpaceMatrix;
    private Vector3 _lightPosition;
    private Vector3 _lightDirection;
    private DirectionalLightShadow _lightShadow;

    public DirectionalShadowMapper(DirectionalLight light)
        : base(new Material(){ Shaders = _shaders })
    {
        _lightShadow = new(light);
        LightEmitter.DirLight = _lightShadow;
        var resolution = new Vector2(1024, 1024);
        CreateDepthMap(resolution);
        CreateLightMatrix(light);
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
        Material.MatricesProperties["lightSpaceMatrix"] = _lightSpaceMatrix;
        _lightShadow.LightSpaceMatrix = _lightSpaceMatrix;
    }

    private void CreateDepthMap(Vector2 resolution)
    {
        FBO.Viewport = resolution;
        if (TextureManager.Get(_depthMapTexture.Hash) is not null)
        {
            TextureManager.Dispose(_depthMapTexture.Hash);
        }

        _depthMapTexture = new Texture2D()
        {
            Size = resolution,
            FilterMode = FilterMode.Nearest,
            WrapMode = WrapMode.ClampToBorder,
            Hash = Guid.NewGuid().ToString(),
            ImageType = ImageType.DeathMap
        };

        _lightShadow.ShadowMap = _depthMapTexture;

        var depthMapTexture = TextureManager.Load(_depthMapTexture);
        depthMapTexture.SetBorderColor(Colors.White);

        FBO.AttachTexture2D(depthMapTexture, FramebufferAttachment.DepthAttachment);
        FBO.Bind();
        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);
        FBO.Unbind();

        FBO.CheckFrameBuffer("DirectionalShadowMapper");
        GlErrorUtils.CheckError("DirectionalShadowMapper CreateDepthMap");
    }

    public override void Dispose(bool disposing)
    {
        if (LightEmitter.DirLight == _lightShadow)
        {
            LightEmitter.DirLight = null;
        }
        base.Dispose(disposing);
    }
}
