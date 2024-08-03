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

    private VerticesInfo _verticesInfo;
    private BufferUsageARB _bufferUsage;

    private readonly GL _gl = Window.GL ?? throw new WindowException("Window.GL is null.");

    public PolygonVAO(float[] vertices, uint[] indices, BufferUsageARB bufferUsage, VerticesInfo verticesInfo)
    {
        _vertices = vertices;
        _indices = indices;
        _verticesInfo = verticesInfo;
        _bufferUsage = bufferUsage;

        Generate();
        GlErrorUtils.CheckError("PolygonVAO");
    }

    private unsafe void Generate()
    {
        Handle = _gl.GenVertexArray();
        _gl.BindVertexArray(Handle);

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        _gl.BufferData<float>(BufferTargetARB.ArrayBuffer, (nuint)(_vertices.Length * sizeof(float)), _vertices, _bufferUsage);

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        _gl.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, (nuint)(_indices.Length * sizeof(uint)), _indices, _bufferUsage);

        var pointer = 0;
        for (uint i = 0; i < _verticesInfo.Size; i++)
        {
            _gl.VertexAttribPointer(i, _verticesInfo.Lengths[i], VertexAttribPointerType.Float, false, _verticesInfo.Stride * sizeof(float), pointer * sizeof(float));
            pointer += _verticesInfo.Lengths[i];
            _gl.EnableVertexAttribArray(i);
        }

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
