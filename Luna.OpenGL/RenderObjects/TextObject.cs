using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class TextObject(TextData data) : RenderObject<TextData>
{
    private const string ProgramName = "FontShader.bin";
    private const string VertexName = "FontVertexShader.glsl";
    private const string FragmentName = "FontFragmentShader.glsl";

    private TextData Text = data;

    private TextVAO TextVAO = new(data);

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
        Program.UniformVec3("textColor", Text.Color.ToMatrix());
        Program.UniformMat4("transform", Text.Transform.Transpose());

        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindVertexArray(TextVAO.Handle);

        float x = 0.0f;
        float y = 0.0f;

        foreach (var c in Text.Text)
        {
            Character ch = TextVAO.Characters[c];

            var yFix = Text.FlipV? TextVAO.Characters['H'].Bearing.Y - ch.Bearing.Y : -(ch.Size.Y - ch.Bearing.Y);

            float xpos = (float)(x + ch.Bearing.X);
            float ypos = (float)(y + yFix );

            float w = ch.Size.X;
            float h = ch.Size.Y;

            _gl.BindTexture(TextureTarget.Texture2D, ch.TextureID);

            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, TextVAO.Vbo);
            var vertices = TextVAO.Vertices(xpos, ypos, w, h, Text.FlipV);
            _gl.BufferSubData<float>(BufferTargetARB.ArrayBuffer, 0, TextVAO.Size, vertices);
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

            _gl.DrawArrays(PrimitiveType.Triangles, 0, 6);

            x += ch.Advance >> 6;

        }

        _gl.BindVertexArray(0);
        _gl.BindTexture(TextureTarget.Texture2D, 0);
    }

    public override void Update(TextData data)
    {
        if (data.FontKey != Text.FontKey)
            TextVAO = new(data);
        Text = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        Program.Dispose();
        TextVAO.Dispose();
    }
}
