using System;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class BufferObject<TDataType> : Disposable
    where TDataType : unmanaged
{
    private readonly uint _handle;
    private readonly BufferTargetARB _bufferType;
    private readonly GL _gl;

    public BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType, BufferUsageARB bufferUsage)
    {
        _gl = gl;
        _bufferType = bufferType;

        _handle = _gl.GenBuffer();
        Bind();
        _gl.BufferData<TDataType>(_bufferType, (nuint)(data.Length * Marshal.SizeOf(typeof(TDataType))), data, bufferUsage);
        GlErrorUtils.CheckError("BufferObject");
    }

    public void Bind()
    {
        _gl.BindBuffer(_bufferType, _handle);
        GlErrorUtils.CheckError("BufferObject Bind");
    }

    public void Unbind()
    {
        _gl.BindBuffer(_bufferType, 0);
        GlErrorUtils.CheckError("BufferObject Unbind");
    }

    public void SubData(uint size, TDataType[] data)
    {
        _gl.BufferSubData<TDataType>(_bufferType, 0, size, data);
        GlErrorUtils.CheckError("BufferObject SubData");
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed)  return;

        _gl.DeleteBuffer(_handle);
        GlErrorUtils.CheckError("BufferObject Dispose");

        base.Dispose(disposing);
    }
}
