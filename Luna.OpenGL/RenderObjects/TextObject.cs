using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class TextObject(TextData data) : RenderObject<TextData>
{
    private const string VertexName = "FontVertexShader.glsl";
    private const string FragmentName = "FontFragmentShader.glsl";

    private TextData _textData = data;

    private TextMesh _textVAO = new(data);

    private readonly ProgramShader Program = new
    (
        [
            new()
            {
                Name = VertexName,
                Path = ProgramShader.DefaultShaderPath(VertexName),
                ShaderType = ShaderType.VertexShader
            },
            new()
            {
                Name = FragmentName,
                Path = ProgramShader.DefaultShaderPath(FragmentName),
                ShaderType = ShaderType.FragmentShader
            }
        ]
    );

    public override void Draw()
    {
        _textVAO.Draw(Program, _textData);
    }

    public override void Update(TextData data)
    {
        if (data.FontKey != _textData.FontKey)
            _textVAO = new(data);
        _textData = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        Program.Dispose();
        _textVAO.Dispose();

        base.Dispose(disposing);
    }
}
