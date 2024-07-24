using System.Numerics;
using FreeTypeSharp;
using Luna.OpenGL.FreeTypeSharp;

namespace Luna.OpenGL;

using FontKey = (string Path, Vector2 Size);

internal class FontManager
{
    private static readonly Dictionary<FontKey, Face> Fonts = [];
    private static readonly Dictionary<FontKey, int> Counters = [];

    public static Face GetFont(FontKey font)
    {
        if (Fonts.TryGetValue(font, out var face))
        {
            return face;
        }

        face = GetFromPath(font);
        return face;
    }

    public static void StartUsing(FontKey fontKey)
    {
        if (!Counters.TryGetValue(fontKey, out _))
            Counters.Add(fontKey, 0);

        Counters[fontKey]++;
    }

    public static void StopUsing(FontKey fontKey)
    {
        if (!Counters.TryGetValue(fontKey, out _))
            return;

        int count = --Counters[fontKey];

        if (count <= 0)
        {
            Counters.Remove(fontKey);
            Fonts.Remove(fontKey);
            GlyphManager.Free(fontKey);
        }
    }

    private static unsafe Face GetFromPath(FontKey font)
    {
        var library = new Library();
        Face face;
        try
        {   
            face = library.CreateFace(font.Path);
        }
        catch(FreeTypeException e)
        {
            throw new ResourceException($"Failed to load font on path: {font.Path}.", e);
        }
        face.SetPixelSizes((uint)font.Size.X, (uint)font.Size.Y);
        Fonts.Add(font, face);
        return face;
    }

}
