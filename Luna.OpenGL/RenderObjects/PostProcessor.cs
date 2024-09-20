using System.Numerics;
using Luna.OpenGL.RenderObjects;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class PostProcessor : FrameBuffer<PostProcessorData>
{
    private readonly IMaterial _material;

    private Texture2D _texture;
    private RenderBufferObject? _rbo;
    private Mesh? _mesh;

    private PostProcessorData _data;

    public PostProcessor(PostProcessorData data)
    {
        _data = data;
        _material = new Material(data.Shaders);
        
        CreatePostProcessor(data.Resolution);
    }

    public override void Draw()
    {
        var size = Injector.Get<IWindow>().Size;
        GL.Viewport(0, 0, (uint)size.X, (uint)size.Y);

        GL.Disable(EnableCap.DepthTest);

        _material.SetTexture2D("SCREEN_TEXTURE", _texture);
        _material.Bind();
        _mesh!.Draw(PrimitiveType.Triangles);
        
        GL.Enable(EnableCap.DepthTest);

        GlErrorUtils.CheckError("PostProcessor Draw");
    }

    public override void Update(PostProcessorData data)
    {
        if (data.Resolution != _data.Resolution)
        {
            _data = data;
            CreatePostProcessor(data.Resolution);
        }
        base.Update(data);
    }

    protected override void Bind(PostProcessorData data)
    {
        GL.Viewport(0, 0, (uint)_data.Resolution.X, (uint)_data.Resolution.Y);
        base.Bind(data);
    }

    private void CreatePostProcessor(Vector2 resolution)
    {
        TextureManager.Get(_texture.Hash)?.Dispose();
        _rbo?.Dispose();
        _mesh?.Dispose();

        var width = (uint)resolution.X;
        var height = (uint)resolution.Y;

        _texture = new Texture2D()
        {
            Size = new Vector2(width, height),
            TextureFilter = TextureFilter.Bilinear,
            TextureWrap = TextureWrap.Clamp,
            MipmapLevel = 0,
            Hash = Guid.NewGuid().ToString()
        };

        _rbo = new(GL, RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, width, height);
        _mesh = new(_vertices, _indices);

        FBO.AttachTexture2D(TextureManager.Load(_texture));
        FBO.AttachRenderBuffer(_rbo);

        GlErrorUtils.CheckFrameBuffer(FramebufferTarget.Framebuffer);
    }

    private static readonly uint[] _indices = 
    [
        0, 1, 2,
        2, 3, 0
    ];

    private readonly static Vertex[] _vertices
        = 
        [
            new Vertex
            {
                Position = new (-1.0f, -1.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 0.0f)
            },
            new Vertex
            {
                Position = new (1.0f, -1.0f, 0.0f), 
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (1.0f, 0.0f)
            },
            new Vertex
            {
                Position = new (1.0f, 1.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (1.0f, 1.0f)
            },
            new Vertex
            {
                Position = new (-1.0f, 1.0f, 0.0f),
                Normal = new (0.0f, 0.0f, 1.0f),
                TexCoords = new (0.0f, 1.0f)
            }
        ];
}

