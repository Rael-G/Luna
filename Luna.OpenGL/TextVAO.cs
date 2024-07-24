using System.Numerics;
using Silk.NET.OpenGL;

namespace Luna.OpenGl;

using FontKey = (string Path, Vector2 Size);

internal class TextVAO
{
    public uint Handle { get; private set; }

    public const uint Size = 6 * 4 * sizeof(float);

    public readonly Dictionary<char, Character> Characters = [];

    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    private readonly FontKey _fontKey;

    private const uint PointerJump = 4 * sizeof(float);
    public uint Vbo { get; private set; }

    public TextVAO(TextData data)
    {
        _fontKey = data.FontKey;
        FontManager.StartUsing(data.FontKey);
        Generate();
    }

    public void Generate()
    {
        _gl.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        for (uint c = 0; c < 128; c++)
        {
            Characters.Add((char)c, GlyphManager.GetGlyph(_fontKey, (char)c));
        }

        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);  

        Handle = _gl.GenVertexArray();
        Vbo = _gl.GenBuffer();
        _gl.BindVertexArray(Handle);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        var _ = 0;
        _gl.BufferData(BufferTargetARB.ArrayBuffer, Size, ref _, BufferUsageARB.DynamicDraw);
        _gl.EnableVertexAttribArray(0);
        _gl.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, PointerJump, 0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindVertexArray(0);
    }

    public static float[] Vertices(float xpos, float ypos, float width, float height, bool flipV)
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

    public void Free()
    {
        _gl.DeleteVertexArray(Handle);
        _gl.DeleteBuffer(Vbo);
        FontManager.StopUsing(_fontKey);
    }

    ~TextVAO()
    {
        Free();
    }
}
