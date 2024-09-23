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

    public FrameBufferObject FBO { get; set; }
    private FrameBufferObject _intermediateFbo;

    private PostProcessorData _data;

    public PostProcessor(PostProcessorData data)
    {
        _data = data;
        _material = new Material(data.Shaders);
        FBO = new(GL, FramebufferTarget.Framebuffer);
        _intermediateFbo = new(GL, FramebufferTarget.Framebuffer);

        CreatePostProcessor(data.Resolution, data.MSAA);
    }

    public override void Draw()
    {
        Bind();
        Injector.Get<IRenderer>().DrawQueue();
        Unbind();

        var size = Injector.Get<IWindow>().Size;
        GL.Viewport(0, 0, (uint)size.X, (uint)size.Y);

        GL.Disable(EnableCap.DepthTest);

        if (_data.MSAA && _multisampleTexture != null)
        {
            _material.SetTexture2D("SCREEN_TEXTURE_MULTI_SAMPLE", (Texture2D)_multisampleTexture);
        }

        _material.SetTexture2D("SCREEN_TEXTURE", _texture);
        
        _material.Bind();
        _mesh!.Draw(PrimitiveType.Triangles);

        GL.Enable(EnableCap.DepthTest);

        GlErrorUtils.CheckError("PostProcessor Draw");
    }

    public override void Update(PostProcessorData data)
    {
        if (data.Resolution != _data.Resolution || data.MSAA != _data.MSAA)
        {
            _data = data;
            CreatePostProcessor(data.Resolution, data.MSAA);
        }
    }

    private void Bind()
    {
        GL.Viewport(0, 0, (uint)_data.Resolution.X, (uint)_data.Resolution.Y);
        FBO.Bind();
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GlErrorUtils.CheckError("PostProcessor Bind");
    }

    private void Unbind()
    {
        if (_data.MSAA && _multisampleTexture != null)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBO.Handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _intermediateFbo.Handle);
            GL.BlitFramebuffer(0, 0, (int)_data.Resolution.X, (int)_data.Resolution.Y, 
                0, 0, (int)_data.Resolution.X, (int)_data.Resolution.Y, 
                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            GlErrorUtils.CheckError("PostProcessor Unbind");
        }
        FBO.Unbind();
    }

    private void CreatePostProcessor(Vector2 resolution, bool msaa)
    {
        TextureManager.Get(_texture.Hash)?.Dispose();
        _rbo?.Dispose();
        _mesh?.Dispose();
        if (_multisampleTexture != null) 
        {
            TextureManager.Get(_multisampleTexture?.Hash!)?.Dispose();
        }
        
        var width = (uint)resolution.X;
        var height = (uint)resolution.Y;

        _mesh = new Mesh(_vertices, _indices);

        _texture = new Texture2D()
        {
            Size = new Vector2(width, height),
            TextureFilter = TextureFilter.Bilinear,
            TextureWrap = TextureWrap.Clamp,
            Hash = Guid.NewGuid().ToString(),
            ImageType = ImageType.Linear
        };

        var texture = GlTexture2D.Create(_texture);
        TextureManager.Cache(_texture.Hash, texture);

        if (msaa)
        {
            var samples = Injector.Get<IWindow>().MSAA;
            _multisampleTexture = new Texture2D()
            {
                Size = new Vector2(width, height),
                TextureFilter = TextureFilter.Bilinear,
                TextureWrap = TextureWrap.Clamp,
                Hash = Guid.NewGuid().ToString(),
                ImageType = ImageType.Linear
            };

            _rbo = new RenderBufferObject(GL, RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, width, height, samples);

            var msTexture = GlTexture2DMultiSample.Create((Texture2D)_multisampleTexture, samples);
            TextureManager.Cache(_multisampleTexture?.Hash!, msTexture);

            FBO.AttachTexture2D(msTexture, FramebufferAttachment.ColorAttachment0);
            FBO.AttachRenderBuffer(_rbo);

            _intermediateFbo.AttachTexture2D(texture, FramebufferAttachment.ColorAttachment0);
        }
        else
        {
            _rbo = new RenderBufferObject(GL, RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, width, height);

            FBO.AttachTexture2D(texture, FramebufferAttachment.ColorAttachment0);
            FBO.AttachRenderBuffer(_rbo);
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
