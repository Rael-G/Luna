using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class Mesh : Disposable
{
    private static readonly GL GL = Window.GL ?? throw new WindowException("Window.GL is null.");

    private readonly Vertex[] _vertices;
    private readonly uint[] _indices;
    private readonly IMaterial _material;

    private readonly BufferUsageARB _bufferUsage;
    private readonly PrimitiveType _primitiveType;

    private readonly uint _size;
    private uint _vao, _vbo, _ebo;

    public Mesh(Vertex[] vertices, uint[] indices, IMaterial material, BufferUsageARB bufferUsage, PrimitiveType primitiveType)
    {
        _vertices = vertices;
        _indices = indices;
        _material = material;
        _bufferUsage = bufferUsage;
        _primitiveType = primitiveType;
        _size = (uint)_indices.Length;

        Setup();
    }

    public void Draw()
    {
        _material.Bind();
        GL.BindVertexArray(_vao);
        var _ = new ReadOnlySpan<int>();
        GL.DrawElements(_primitiveType, _size, DrawElementsType.UnsignedInt, _);

        GL.BindVertexArray(0);

        GlErrorUtils.CheckError("Mesh Draw");
    }

    private void Setup()
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        GL.BufferData<Vertex>(BufferTargetARB.ArrayBuffer, (nuint)(_vertices.Length * Vertex.Stride), _vertices, _bufferUsage);

        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        GL.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, (nuint)(_indices.Length * sizeof(uint)), _indices, _bufferUsage);

        var pointer = 0;
        for (uint i = 0; i < Vertex.Params; i++)
        {
            GL.VertexAttribPointer(i, Vertex.Lengths[i], VertexAttribPointerType.Float, false, Vertex.Stride, pointer * sizeof(float));
            pointer += Vertex.Lengths[i];
            GL.EnableVertexAttribArray(i);
        }

        GlErrorUtils.CheckVao(_vao);
        GlErrorUtils.CheckVbo(_vbo);
        GlErrorUtils.CheckEbo(_ebo);

        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

        GlErrorUtils.CheckError("Mesh Setup");
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);

        base.Dispose(disposing);
    }
}
