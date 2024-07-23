using System.Numerics;
using Luna.OpenGl.FreeTypeSharp;
using Silk.NET.OpenGL;

namespace Luna.OpenGl;

using FontKey = (string Path, Vector2 Size);

public static class GlyphManager
{
    private static readonly Dictionary<FontKey, Dictionary<char, Character>> Glyphs = new();

    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    public static Character GetGlyph(FontKey fontKey, char c)
    {
        if (!Glyphs.TryGetValue(fontKey, out var charCache))
        {
            charCache = [];
            Glyphs[fontKey] = charCache;
        }

        if (!charCache.TryGetValue(c, out var character))
        {
            var face = FontManager.GetFont(fontKey);
            face.LoadChar(c, LoadFlags.Render);
            var bitmap = face.Glyph.Bitmap;

            var texture = _gl.GenTexture();
            _gl.BindTexture(TextureTarget.Texture2D, texture);

            _gl.TexImage2D<byte>(TextureTarget.Texture2D, 0, InternalFormat.R8, bitmap.Width,
                bitmap.Rows, 0, PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

            _gl.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            _gl.TextureParameter(texture, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            _gl.TextureParameter(texture, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
            _gl.TextureParameter(texture, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

            character = new Character
            {
                TextureID = texture,
                Size = new Vector2(bitmap.Width, bitmap.Rows),
                Bearing = new Vector2(face.Glyph.BitmapLeft, face.Glyph.BitmapTop),
                Advance = (uint)face.Glyph.Advance.X
            };

            charCache[c] = character;
        }

        return character;
    }

    internal static void Free(FontKey font)
    {
        var fontCache = Glyphs[font];
        foreach (var character in fontCache.Values)
        {
            Window.GL?.DeleteTexture(character.TextureID);
        }
    }

}
