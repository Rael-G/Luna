using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGl;

internal class RectangleVAO
{
    public uint Handle { get; private set; }

    private readonly Vector2 _size;

    private uint _vbo;
    private uint _ebo;

    private float[] Vertices => 
    [
        0.0f, 0.0f, 0.0f,  // Bottom left
        0.0f, _size.Y, 0.0f,  // Top left
        _size.X, _size.Y, 0.0f,   //Top right
        _size.X, 0.0f, 0.0f,    // Bottom right
    ];

    private static readonly uint[] Indices = 
    [
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    ];

    private readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    public RectangleVAO(Vector2 size)
    {
        _size = size;
        Generate();
    }

    public static uint Size => (uint)Indices.Length;

    private unsafe void Generate()
    {
        Handle = _gl.GenVertexArray();
        _gl.BindVertexArray(Handle);

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        _gl.BufferData<float>(BufferTargetARB.ArrayBuffer, (nuint)Vertices.Length * sizeof(float), Vertices, BufferUsageARB.StaticDraw);

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        _gl.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, (nuint)Indices.Length * sizeof(uint), Indices, BufferUsageARB.StaticDraw);

        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
        _gl.EnableVertexAttribArray(0);

        GlErrorUtils.CheckVao(Handle);
        GlErrorUtils.CheckVbo(_vbo);
        GlErrorUtils.CheckEbo(_ebo);

        _gl.BindVertexArray(0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
    }

    ~RectangleVAO()
    {
        _gl.DeleteVertexArray(Handle);
        _gl.DeleteBuffer(_vbo);
        _gl.DeleteBuffer(_ebo);
    }
    
}
