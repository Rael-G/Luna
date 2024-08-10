using System;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class BufferObject<TDataType> : Disposable
    where TDataType : unmanaged
{
    private readonly uint _handle;
    private BufferTargetARB _bufferType;
    private readonly GL _gl;

    public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType, BufferUsageARB bufferUsage)
    {
        _gl = gl;
        _bufferType = bufferType;

        _handle = _gl.GenBuffer();
        Bind();
        _gl.BufferData<TDataType>(_bufferType, (nuint)(data.Length * sizeof(TDataType)), data, bufferUsage);
    }

    public void Bind()
    {
        _gl.BindBuffer(_bufferType, _handle);
    }

    public void Unbind()
    {
        _gl.BindBuffer(_bufferType, 0);
    }

    public void SubData(uint size, TDataType[] data)
    {
        _gl.BufferSubData<TDataType>(_bufferType, 0, size, data);
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed)  return;

        _gl.DeleteBuffer(_handle);

        base.Dispose(disposing);
    }
}
