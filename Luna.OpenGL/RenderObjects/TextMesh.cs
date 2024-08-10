using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

using FontKey = (string Path, Vector2 Size);

internal class TextMesh : Disposable
{
    private const uint Size = 6 * 4 * sizeof(float);
    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    private readonly VertexArrayObject<float, uint> _vao;
    private readonly BufferObject<float> _vbo;
    private readonly Dictionary<char, Character> _characters = [];
    private readonly FontKey _fontKey;

    public TextMesh(TextData data)
    {
        _fontKey = data.FontKey;
        FontManager.StartUsing(data.FontKey);

        _gl.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        for (uint c = 0; c < 128; c++)
        {
            _characters.Add((char)c, GlyphManager.GetGlyph(_fontKey, (char)c));
        }
        
        _vbo = new BufferObject<float>(_gl, new float[6 * 4], BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw);
        _vao = new VertexArrayObject<float, uint>(_gl, _vbo);
        _vao.VertexAttributePointer(0, 4, VertexAttribPointerType.Float, 4 * sizeof(float), 0);

        GlErrorUtils.CheckError("TextVAO");
    }

    public void Draw(ProgramShader shader, TextData text)
    {
        shader.Use();
        shader.SetVec4("textColor", text.Color.ToMatrix());
        shader.SetMat4("transform", text.Transform.Transpose());

        _gl.ActiveTexture(TextureUnit.Texture0);
        _vao.Bind();

        float x = 0.0f;
        float y = 0.0f;

        foreach (var c in text.Text)
        {
            Character ch = _characters[c];

            var yFix = text.FlipV? _characters['H'].Bearing.Y - ch.Bearing.Y : -(ch.Size.Y - ch.Bearing.Y);

            float xpos = (float)(x + ch.Bearing.X);
            float ypos = (float)(y + yFix );

            float w = ch.Size.X;
            float h = ch.Size.Y;

            _gl.BindTexture(TextureTarget.Texture2D, ch.TextureID);

            var vertices = Vertices(xpos, ypos, w, h, text.FlipV);
            _vbo.Bind();
            _vbo.SubData(Size, vertices);
            _gl.DrawArrays(PrimitiveType.Triangles, 0, 6);

            x += ch.Advance >> 6;
        }

        _gl.BindTexture(TextureTarget.Texture2D, 0);
        GlErrorUtils.CheckError("TextObject");
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        _vao.Dispose();
        _vbo.Dispose();
        FontManager.StopUsing(_fontKey);

        base.Dispose(disposing);
    }

    private static float[] Vertices(float xpos, float ypos, float width, float height, bool flipV)
    {
        var bottom = flipV? 1.0f : 0.0f;
        var top = flipV? 0.0f : 1.0f;
        return [
            xpos,           ypos + height,  0.0f, bottom,
            xpos,           ypos,           0.0f, top,
            xpos + width,   ypos,           1.0f, top,
            xpos,           ypos + height,  0.0f, bottom,
            xpos + width,   ypos,           1.0f, top,
            xpos + width,   ypos + height,  1.0f, bottom
        ];
    }
}
