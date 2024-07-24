using Silk.NET.OpenGL;

namespace Luna.OpenGl;

internal class RectangleObject(RectangleData data) : RenderObject<RectangleData>
{
    private const string ProgramName = "RectangleShader.bin";
    private const string VertexName = "RectangleVertexShader.glsl";
    private const string FragmentName = "RectangleFragmentShader.glsl";

    private RectangleData Data = data;

    private RectangleVAO RectangleVAO = new(data.Size);

    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    private readonly Program Program = new
    (
        ProgramName,
        [
            new()
            {
                Name = VertexName,
                Path = Program.DefaultShaderPath(VertexName),
                ShaderType = ShaderType.VertexShader
            },
            new()
            {
                Name = FragmentName,
                Path = Program.DefaultShaderPath(FragmentName),
                ShaderType = ShaderType.FragmentShader
            }
        ]
    );

    public override void Render()
    {
        Program.Use();
        Program.UniformMat4("transform", Data.Transform.Transpose());
        Program.UniformVec4("color", Data.Color.ToMatrix());
        _gl.BindVertexArray(RectangleVAO.Handle);
        ReadOnlySpan<int> _ = new();
        _gl.DrawElements(PrimitiveType.Triangles, RectangleVAO.Size, DrawElementsType.UnsignedInt, _);
        _gl.BindVertexArray(0);

        GlErrorUtils.CheckError();
    }

    public override void Update(RectangleData data)
    {
        if (Data.Size != data.Size) 
        {
            RectangleVAO.Free();
            RectangleVAO = new(Data.Size);
        }

        Data = data;
    }

    public override void Free()
    {
        RectangleVAO.Free();
        Program.Free();
    }
}
