using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class VertexArrayObject<TVertexType, TIndexType> : Disposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        private readonly uint _handle;
        private readonly GL _gl;

        public VertexArrayObject(GL gl, BufferObject<TVertexType> vbo, BufferObject<TIndexType>? ebo = null)
        {
            _gl = gl;
            _handle = _gl.GenVertexArray();
            Bind();
            vbo.Bind();
            ebo?.Bind();
            GlErrorUtils.CheckError("VertexArrayObject");
        }

        public void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint stride, int pointer)
        {
            Bind();
            _gl.VertexAttribPointer(index, count, type, false, stride, pointer);
            _gl.EnableVertexAttribArray(index);
            Unbind();

            GlErrorUtils.CheckError("VertexArrayObject VertexAttributePointer");
        }

        public void Bind()
        {
            _gl.BindVertexArray(_handle);
            GlErrorUtils.CheckError("VertexArrayObject Bind");
        }

        public void Unbind()
        {
            _gl.BindVertexArray(0);
            GlErrorUtils.CheckError("VertexArrayObject Unbind");
        }

        public override void Dispose(bool disposing)
        {
            if (_disposed)  return;

            _gl.DeleteVertexArray(_handle);
            GlErrorUtils.CheckError("VertexArrayObject Dispose");

            base.Dispose(disposing);
        }
    }
