using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal abstract class PolygonObject<TData> : RenderObject<TData>
{
    private const string VertexName = "PolygonVertexShader.glsl";
    private const string FragmentName = "PolygonFragmentShader.glsl";

    private PolygonData _polygonData;
    private PolygonVAO _polygonVAO;

    protected Program Program { get; set;}

    public PolygonObject(PolygonData data)
    {
        _polygonData = data;
        _polygonVAO = new PolygonVAO(_polygonData.Vertices, _polygonData.Indices);

        Program = data.Shader is not null? 
        new Program(data.Shader) :
        new Program
        (
            [
                new()
                {
                    Name = VertexName,
                    Path = Program.DefaultShaderPath(VertexName),
                    ShaderType = Core.ShaderType.VertexShader
                },
                new()
                {
                    Name = FragmentName,
                    Path = Program.DefaultShaderPath(FragmentName),
                    ShaderType = Core.ShaderType.FragmentShader
                }
            ]
        );
    }

    public override void Render()
    {
        Program.Use();
        Program.UniformMat4("transform", _polygonData.Transform.Transpose());
        Program.UniformVec4("color", _polygonData.Color.ToMatrix());
        GL.BindVertexArray(_polygonVAO.Handle);
        ReadOnlySpan<int> _ = new();
        GL.DrawElements(_polygonData.PrimitiveType, _polygonVAO.Size, DrawElementsType.UnsignedInt, _);

        GL.BindVertexArray(0);

        GlErrorUtils.CheckError();
    }

    public void Update(PolygonData data)
    {
        if (_polygonData.Vertices != data.Vertices || _polygonData.Indices != data.Indices) 
        {
            _polygonVAO.Dispose();
            _polygonVAO = new(data.Vertices, data.Indices);
        }
        _polygonData = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        _polygonVAO.Dispose();
        Program.Dispose();
    }
}
