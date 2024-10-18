using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class DepthMap 
{
    GL _gl;
    string _data;
    Material _material;
    ShaderSource[] _shaders;
    FrameBufferObject _fbo { get; set; }
    bool useCulling;

    Vector2 _size;
    public Texture2D DepthMapTexture { get; private set; }

    public DepthMap(Vector2 size, GL gl)
    {
        _gl = gl;
        _size = size;
        _material = new Material()
        {
            Shaders = _shaders
        };
        _fbo = new(gl, FramebufferTarget.Framebuffer);
    }

    private void Bind()
    {
        _gl.Viewport(0, 0, (uint)_size.X, (uint)_size.Y);
        _fbo.Bind();
        _gl.Clear(ClearBufferMask.DepthBufferBit);

        var window = Injector.Get<IWindow>();
        useCulling = window.Flags.HasFlag(WindowFlags.BackFaceCulling);
        window.Flags |= WindowFlags.BackFaceCulling;
        _gl.CullFace(TriangleFace.Front);

        GlErrorUtils.CheckError("PostProcessor Bind");
    }

    private void Unbind()
    {
        _gl.CullFace(TriangleFace.Back);
        if (!useCulling)
        {
            Injector.Get<IWindow>().Flags &= ~WindowFlags.BackFaceCulling;
        }

        _fbo.Unbind();

        var size = Injector.Get<IWindow>().Size;
        _gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
        _gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        GlErrorUtils.CheckError("PostProcessor Unbind");
    }

    public void CreateDepthMap()
    {
        TextureManager.Get(DepthMapTexture.Hash)?.Dispose();
        
        var width = (uint)_size.X;
        var height = (uint)_size.Y;

        DepthMapTexture = new Texture2D()
        {
            Size = new Vector2(width, height),
            TextureFilter = TextureFilter.Nearest,
            TextureWrap = TextureWrap.Repeat,
            Hash = Guid.NewGuid().ToString(),
            ImageType = ImageType.DeathMap
        };

        var texture = GlTexture2D.Create(DepthMapTexture);
        TextureManager.Cache(DepthMapTexture.Hash, texture);

        _fbo.AttachTexture2D(texture, FramebufferAttachment.DepthAttachment);
        _fbo.Bind();
        _gl.DrawBuffer(DrawBufferMode.None);
        _gl.ReadBuffer(ReadBufferMode.None);
        _fbo.Unbind();
    
        GlErrorUtils.CheckFrameBuffer(FramebufferTarget.Framebuffer);
    }

}
