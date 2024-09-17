using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class Mesh : Disposable
{
    private static readonly GL GL = Window.GL ?? throw new WindowException("Window.GL is null.");
    private static readonly uint _stride = (uint)Marshal.SizeOf(typeof(Vertex));

    private readonly VertexArrayObject<Vertex, uint> _vao;
    private readonly BufferObject<uint> _ebo;
    private readonly BufferObject<Vertex> _vbo;
    
    private readonly uint _size;

    public Mesh(Vertex[] vertices, uint[] indices)
    {
        _size = (uint)indices.Length;

        _ebo = new BufferObject<uint>(GL, indices, BufferTargetARB.ElementArrayBuffer, BufferUsageARB.StaticDraw);
        _vbo = new BufferObject<Vertex>(GL, vertices, BufferTargetARB.ArrayBuffer, BufferUsageARB.StaticDraw);
        _vao = new VertexArrayObject<Vertex, uint>(GL, _vbo, _ebo);

        _vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, _stride, 0 * sizeof(float));
        _vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, _stride, 3 * sizeof(float));
        _vao.VertexAttributePointer(2, 2, VertexAttribPointerType.Float, _stride, 6 * sizeof(float));

        GlErrorUtils.CheckError("Mesh Setup");
    }

    public void Draw(PrimitiveType primitiveType)
    {
        _vao.Bind();
        GL.DrawElements(primitiveType, _size, DrawElementsType.UnsignedInt, new ReadOnlySpan<int>());
        GlErrorUtils.CheckError("Mesh Draw");
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        _vao.Dispose();
        _vbo.Dispose();
        _ebo.Dispose();
        
        base.Dispose(disposing);
    }
}
