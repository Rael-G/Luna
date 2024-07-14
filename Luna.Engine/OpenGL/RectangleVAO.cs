using Luna.Core;
using Silk.NET.OpenGL;

namespace Luna.Engine.OpenGl;

internal class RectangleVAO
{
    public uint Id { get; private set; }

    private readonly float _width;
    private readonly float _height;

    private uint _vbo;
    private uint _ebo;

    private float[] Vertices => 
    [
        0.0f, 0.0f, 0.0f,  // Bottom left
        0.0f, _height, 0.0f,  // Top left
        _width, _height, 0.0f,   //Top right
        _width, 0.0f, 0.0f,    // Bottom right
    ];

    private static readonly uint[] Indices = 
    [
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    ];

    private readonly GL _gl = Window.Gl?? throw new WindowException("Window.Gl is null.");

    public RectangleVAO(float width, float height)
    {
        _width = width;
        _height = height;
        Generate();
    }

    public static uint Size()
        => (uint)Indices.Length;

    private unsafe void Generate()
    {
        Id = _gl.GenVertexArray();
        _gl.BindVertexArray(Id);

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        _gl.BufferData<float>(BufferTargetARB.ArrayBuffer, (nuint)Vertices.Length * sizeof(float), Vertices, BufferUsageARB.StaticDraw);

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        _gl.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, (nuint)Indices.Length * sizeof(uint), Indices, BufferUsageARB.StaticDraw);

        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
        _gl.EnableVertexAttribArray(0);

        GlErrorUtils.CheckVao(Id);
        GlErrorUtils.CheckVbo(_vbo);
        GlErrorUtils.CheckEbo(_ebo);

        _gl.BindVertexArray(0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
    }

    ~RectangleVAO()
    {
        _gl.DeleteVertexArray(Id);
        _gl.DeleteBuffer(_vbo);
        _gl.DeleteBuffer(_ebo);
    }
    
}
