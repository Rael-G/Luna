using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal abstract class PolygonObject<TData> : RenderObject<TData>
{
    private PolygonData _polygonData;
    private PolygonVAO _polygonVAO;

    public PolygonObject(PolygonData data)
    {
        _polygonData = data;
        _polygonVAO = new PolygonVAO(_polygonData.Vertices, _polygonData.Indices, _polygonData.BufferUsage, _polygonData.VerticeInfo);
    }

    public override void Render()
    {
        _polygonData.Material.Bind();
        GL.BindVertexArray(_polygonVAO.Handle);
        var _ = new ReadOnlySpan<int>();
        GL.DrawElements(_polygonData.PrimitiveType, _polygonVAO.Size, DrawElementsType.UnsignedInt, _);

        GL.BindVertexArray(0);

        GlErrorUtils.CheckError("PolygonObject");
    }

    public void Update(PolygonData data)
    {
        if (!_polygonData.Vertices.SequenceEqual(data.Vertices))
        {
            _polygonVAO.Dispose();
            _polygonVAO = new(data.Vertices, data.Indices, data.BufferUsage, data.VerticeInfo);
        }
        _polygonData = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        _polygonVAO.Dispose();
        _polygonData.Material.Dispose();

        base.Dispose(disposing);
    }
}
