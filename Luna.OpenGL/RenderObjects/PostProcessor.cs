using Luna.OpenGL.RenderObjects;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class PostProcessor : FrameBuffer<PostProcessorData>
{
    private static readonly uint[] _indices = 
    [
        0, 1, 2,   // first triangle
        2, 3, 0    // second triangle
    ];

    private readonly Material _material;

    private readonly Texture _texture;
    private readonly RenderBufferObject _rbo;
    private readonly Mesh _mesh;

    public PostProcessor(PostProcessorData data)
    {
        _material = new(data.Shaders);
        var width = (uint)Injector.Get<IWindow>().Size.X;
        var height = (uint)Injector.Get<IWindow>().Size.Y;
        _texture = Texture.Load(width, height, TextureFilter.Bilinear, TextureWrap.Clamp, 0, TextureTarget.Texture2D);
        _rbo = new(GL, RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, width, height);

        FBO.AttachTexture2D(_texture);
        FBO.AttachRenderBuffer(_rbo);

        _mesh = new(GetVertices(), _indices);

        GlErrorUtils.CheckFrameBuffer(FramebufferTarget.Framebuffer);
    }

    public override void Draw()
    {
        GL.Disable(EnableCap.DepthTest);

        _material.Set("SCREEN_TEXTURE", _texture);
        _material.Bind();
        _mesh.Draw(PrimitiveType.Triangles);
        
        GL.Enable(EnableCap.DepthTest);

        GlErrorUtils.CheckError("PostProcessor Draw");

    }

    private static Vertex[] GetVertices()
        => 
        [
            new Vertex  // Bottom left
            {
                Position = new (-1.0f, -1.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 0.0f)
            },
            new Vertex // Bottom right
            {
                Position = new (1.0f, -1.0f, 0.0f), 
                Normal = new (-1.0f, -1.0f, 1.0f),
                TexCoords = new (1.0f, 0.0f)
            },
            new Vertex // Top right
            {
                Position = new (1.0f, 1.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (1.0f, 1.0f)
            },
            new Vertex  // Top left
            {
                Position = new (-1.0f, 1.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 1.0f)
            }
        ];
}

