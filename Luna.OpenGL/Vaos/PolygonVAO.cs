using System.Numerics;
using Luna.Core;
using Luna.Maths;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class PolygonVAO : Disposable
{
    public uint Handle { get; private set; }
    public uint Size => (uint)_indices.Length;

    private readonly float[] _vertices;
    private readonly uint[] _indices;
    private uint _vbo;
    private uint _ebo;

    private readonly GL _gl = Window.GL ?? throw new WindowException("Window.GL is null.");

    public PolygonVAO(float[] vertices, uint[] indices)
    {
        _vertices = vertices;
        _indices = indices;

        Generate();
    }

    private unsafe void Generate()
    {
        Handle = _gl.GenVertexArray();
        _gl.BindVertexArray(Handle);

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        _gl.BufferData<float>(BufferTargetARB.ArrayBuffer, (nuint)(_vertices.Length * sizeof(float)), _vertices, BufferUsageARB.StaticDraw);

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        _gl.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, (nuint)(_indices.Length * sizeof(uint)), _indices, BufferUsageARB.StaticDraw);

        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
        _gl.EnableVertexAttribArray(0);

        GlErrorUtils.CheckVao(Handle);
        GlErrorUtils.CheckVbo(_vbo);
        GlErrorUtils.CheckEbo(_ebo);

        _gl.BindVertexArray(0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        _gl.DeleteVertexArray(Handle);
        _gl.DeleteBuffer(_vbo);
        _gl.DeleteBuffer(_ebo);
    }
}
