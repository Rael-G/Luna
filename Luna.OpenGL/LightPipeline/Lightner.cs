using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class LightEmitter
{
    public static DirectionalLightShadow? DirLight { get; set; }
    public static readonly List<PointLightShadow> PointLights = [];
    public static readonly List<SpotLightShadow> SpotLights = [];

    protected static readonly GL GL = Window.GL ?? throw new WindowException("Window.GL is null.");

    private const uint MAX_LIGHTS = 1024;

    private uint _lightBuffer;
    private uint _visibleLightIndicesBuffer;
    private static readonly Vector2 SCREEN_SIZE = new (1920, 1080);

    private static readonly uint workGroupsX = (uint)(SCREEN_SIZE.X + (SCREEN_SIZE.X % 16)) / 16;
	private static readonly uint workGroupsY = (uint)(SCREEN_SIZE.Y + (SCREEN_SIZE.Y % 16)) / 16;
    private static readonly uint TILES = workGroupsX * workGroupsY;

    FrameBufferObject FBO;
    bool _useCulling;
    public Texture2D _depthMapTexture;

    private const string VertexName = "depth.vertex.glsl";
    private const string FragmentName = "depth.vertex.glsl";

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

    private static readonly Material Material = new(){ Shaders = _shaders };

    public LightEmitter()
    {
        _lightBuffer = GL.GenBuffer();
        _visibleLightIndicesBuffer = GL.GenBuffer();

        GL.BindBuffer(BufferTargetARB.ShaderStorageBuffer, _lightBuffer);
        GL.BufferData<PointLightGL>(BufferTargetARB.ShaderStorageBuffer, (nuint)(Marshal.SizeOf(typeof(PointLight)) * MAX_LIGHTS), null, BufferUsageARB.DynamicDraw);

        GL.BindBuffer(BufferTargetARB.ShaderStorageBuffer, _visibleLightIndicesBuffer);
        GL.BufferData<PointLightGL>(BufferTargetARB.ShaderStorageBuffer, (nuint)(Marshal.SizeOf(typeof(VisibleIndex)) * TILES * MAX_LIGHTS), null, BufferUsageARB.DynamicDraw);

        FBO = new(GL, FramebufferTarget.Framebuffer);

        CreateDepthMap();
    }

    public void DephtPrePass()
    {
        Bind();
        Injector.Get<IRenderer>().DrawQueue(Material, false);
        Unbind();
    }

    private void Bind()
    {
        FBO.Bind();
        GL.Clear(ClearBufferMask.DepthBufferBit);

        var window = Injector.Get<IWindow>();
        _useCulling = window.Flags.HasFlag(WindowFlags.BackFaceCulling);
        GL.CullFace(TriangleFace.Front);

        GlErrorUtils.CheckError("LightEmitter Bind");
    }

    private void Unbind()
    {
        GL.CullFace(TriangleFace.Back);
        if (!_useCulling)
        {
            Injector.Get<IWindow>().Flags &= ~WindowFlags.BackFaceCulling;
        }

        FBO.Unbind();

        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        GlErrorUtils.CheckError("LightEmitter Unbind");
    }

    private void CreateDepthMap()
    {
        var resolution = Injector.Get<IWindow>().Size;
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

        var depthMapTexture = TextureManager.Load(_depthMapTexture);
        depthMapTexture.SetBorderColor(Colors.White);

        FBO.AttachTexture2D(depthMapTexture, FramebufferAttachment.DepthAttachment);
        FBO.Bind();
        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);
        FBO.Unbind();

        FBO.CheckFrameBuffer();
        GlErrorUtils.CheckError("LightEmitter CreateDepthMap");
    }
}

public struct PointLightGL
{
    public Vector3 Position;
    
    public float Constant;
    public float Linear;
    public float Quadratic;
	
    public Vector3 Ambient;
    public Vector3 Diffuse;
    public Vector3 Specular;

    public float FarPlane;
    public int ShadowMap;
}

public struct VisibleIndex
{
    public int Index;
}

