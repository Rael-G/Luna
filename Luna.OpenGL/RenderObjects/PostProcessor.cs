using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class PostProcessor :  RenderObject<PostProcessorData>
{
    private readonly IMaterial _material;

    private Texture2D _texture;
    private Texture2D? _multisampleTexture;
    private RenderBufferObject? _rbo;
    private Mesh? _mesh;

    public FrameBufferObject _fbo { get; set; }
    private FrameBufferObject _intermediateFbo;

    private PostProcessorData _data;

    private bool _draw = true;

    public PostProcessor(PostProcessorData data)
    {
        _data = data;
        _material = new Material()
        {
            Shaders = data.Shaders
        };
        _fbo = new(GL, FramebufferTarget.Framebuffer);
        _intermediateFbo = new(GL, FramebufferTarget.Framebuffer);
        CreatePostProcessor(data.Resolution, data.Samples);

        Priority = 2;
    }

    public override void Draw()
    {
        // Avoid infinite recursion
        if (!_draw)
        {
            return;
        }
        _draw = false;

        // Draw into the fbo
        Bind();
        Injector.Get<IRenderer>().DrawQueue();
        Unbind();

        _draw = true;

        GL.Disable(EnableCap.DepthTest);

        if (_data.Samples > 0 && _multisampleTexture != null)
        {
            _material.SetTexture2D("SCREEN_TEXTURE_MULTI_SAMPLE", (Texture2D)_multisampleTexture);
        }

        _material.SetTexture2D("SCREEN_TEXTURE", _texture);
        
        //Draw to the screen
        _material.Bind();
        _mesh!.Draw(PrimitiveType.Triangles);
        _material.Unbind();

        GL.Enable(EnableCap.DepthTest);

        Renderer.Interrupt();

        GlErrorUtils.CheckError("PostProcessor Draw");
    }

    public override void Draw(IMaterial material)
    {
        // Do nothing
    }

    public override void Update(PostProcessorData data)
    {
        if (data.Resolution != _data.Resolution || data.Samples != _data.Samples)
        {
            _data = data;
            CreatePostProcessor(data.Resolution, data.Samples);
        }
    }

    private void Bind()
    {
        _fbo.Bind();
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GlErrorUtils.CheckError("PostProcessor Bind");
    }

    private void Unbind()
    {
        if (_data.Samples > 0 && _multisampleTexture != null)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _fbo.Handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _intermediateFbo.Handle);
            GL.BlitFramebuffer(0, 0, (int)_data.Resolution.X, (int)_data.Resolution.Y, 
                0, 0, (int)_data.Resolution.X, (int)_data.Resolution.Y, 
                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
        }
        _fbo.Unbind();

        GlErrorUtils.CheckError("PostProcessor Unbind");
    }

    private void CreatePostProcessor(Vector2 resolution, int samples)
    {
        _fbo.Viewport = resolution;
        _intermediateFbo.Viewport = resolution;

        if (TextureManager.Get(_texture.Hash) is not null)
        {
            TextureManager.Dispose(_texture.Hash);
        }
        if (_multisampleTexture != null) 
        {
            if (TextureManager.Get(_multisampleTexture.Value.Hash) is not null)
            {
                TextureManager.Dispose(_multisampleTexture.Value.Hash);
            }
        }
        _rbo?.Dispose();
        _mesh?.Dispose();

        var width = (uint)resolution.X;
        var height = (uint)resolution.Y;

        _mesh = new Mesh(_vertices, _indices);

        _texture = new Texture2D()
        {
            Size = resolution,
            FilterMode = FilterMode.Bilinear,
            WrapMode = WrapMode.ClampToEdge,
            Hash = Guid.NewGuid().ToString(),
            ImageType = ImageType.Linear
        };

        var texture = TextureManager.Load(_texture);
        if (samples > 0)
        {
            _multisampleTexture = new Texture2D()
            {
                Size = resolution,
                FilterMode = FilterMode.Bilinear,
                WrapMode = WrapMode.ClampToEdge,
                Hash = Guid.NewGuid().ToString(),
                ImageType = ImageType.Linear
            };

            _rbo = new RenderBufferObject(GL, RenderbufferTarget.Renderbuffer, 
                InternalFormat.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, 
                    width, height, samples);

            var msTexture = new Texture2DGL(GL, _multisampleTexture.Value.Size, 
                _multisampleTexture.Value.ImageType, _multisampleTexture.Value.FilterMode, 
                _multisampleTexture.Value.WrapMode, 0, samples);
            TextureManager.Cache(_multisampleTexture.Value.Hash, msTexture);

            _fbo.AttachTexture2D(msTexture, FramebufferAttachment.ColorAttachment0);
            _fbo.AttachRenderBuffer(_rbo);
            _fbo.CheckFrameBuffer("PostProcessor CreatePostProcessor MultiSample");

            _intermediateFbo.AttachTexture2D(texture, FramebufferAttachment.ColorAttachment0);
            _intermediateFbo.CheckFrameBuffer("PostProcessor CreatePostProcessor MultiSample Intermediate");
        }
        else
        {
            _rbo = new RenderBufferObject(GL, RenderbufferTarget.Renderbuffer, 
                InternalFormat.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, 
                width, height);

            _fbo.AttachTexture2D(texture, FramebufferAttachment.ColorAttachment0);
            _fbo.AttachRenderBuffer(_rbo);
            _fbo.CheckFrameBuffer("PostProcessor CreatePostProcessor");
        }

        GlErrorUtils.CheckFrameBuffer(FramebufferTarget.Framebuffer);
    }

    private static readonly uint[] _indices =
    [
        0, 1, 2,
        2, 3, 0
    ];

    private readonly static Vertex[] _vertices =
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
