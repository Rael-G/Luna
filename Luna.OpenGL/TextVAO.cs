using System.Numerics;
using Luna.OpenGl.FreeTypeSharp;
using Silk.NET.OpenGL;

namespace Luna.OpenGl;

internal class TextVAO
{
    public uint Handle { get; private set; }

    public const uint Size = 6 * 4 * sizeof(float);

    public readonly Dictionary<char, Character> Characters = [];

    private readonly GL _gl = Window.Gl?? throw new WindowException("Window.Gl is null.");

    private readonly Face Font;

    private const uint PointerJump = 4 * sizeof(float);
    public uint Vbo { get; private set; }

    public TextVAO(TextData data)
    {
        Font = FontManager.GetFont(data);
        Font.SetPixelSizes(0, (uint)data.PixelSize);
        Generate();
    }

    public void Generate()
    {
        _gl.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        for (uint c = 0; c < 128; c++)
        {

            Font.LoadChar((char)c, LoadFlags.Render);
            var bitmap = Font.Glyph.Bitmap;
            
            var texture = _gl.GenTexture();
            _gl.BindTexture(TextureTarget.Texture2D, texture);

            _gl.TexImage2D<byte>(TextureTarget.Texture2D, 0, InternalFormat.R8, bitmap.Width, 
                bitmap.Rows, 0, PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

            _gl.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            _gl.TextureParameter(texture, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            _gl.TextureParameter(texture, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
            _gl.TextureParameter(texture, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            
            Character character = new()
            {
                TextureID = texture,
                Size = new Vector2(bitmap.Width, bitmap.Rows),
                Bearing = new Vector2(Font.Glyph.BitmapLeft, Font.Glyph.BitmapTop),
                Advance = (uint)Font.Glyph.Advance.X
            };

            Characters.Add((char)c, character);
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

    ~TextVAO()
    {
        _gl.DeleteVertexArray(Handle);
        _gl.DeleteBuffer(Vbo);
    }
}
