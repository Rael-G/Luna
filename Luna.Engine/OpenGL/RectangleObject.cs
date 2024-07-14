using Luna.Core;
using Luna.Maths;
using Silk.NET.OpenGL;

namespace Luna.Engine.OpenGl;

internal class RectangleObject(RectangleData data) : RenderObject<RectangleData>
{
    private const string ProgramName = "RectangleShader.bin";
    private const string VertexName = "RectangleVertexShader.glsl";
    private const string FragmentName = "RectangleFragmentShader.glsl";

    private RectangleData Data { get; set; } = data;

    private RectangleVAO RectangleVAO { get; set; } = new RectangleVAO((float)data.Width, (float)data.Height);

    private static readonly GL _gl = Window.Gl?? throw new WindowException("Window.Gl is null.");

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
        Program.UniformMat4("transform", Data.Transform.AsSpan());
        Program.UniformVec4("color", ((Matrix)Data.Color).AsSpan());
        _gl.BindVertexArray(RectangleVAO.Id);
        ReadOnlySpan<int> _ = new();
        _gl.DrawElements(PrimitiveType.Triangles, RectangleVAO.Size(), DrawElementsType.UnsignedInt, _);
        _gl.BindVertexArray(0);

        GlErrorUtils.CheckError();
    }

    public override void Update(RectangleData data)
    {
        if (Data.Width != data.Width || Data.Height != data.Height)
            RectangleVAO = new((float)data.Width, (float)data.Height);

        Data = data;
    }
}
