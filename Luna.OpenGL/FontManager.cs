using FreeTypeSharp;
using Luna.OpenGl.FreeTypeSharp;

namespace Luna.OpenGl;

internal class FontManager
{
    private static readonly Dictionary<string, Face> Fonts = [];
    private static readonly Dictionary<string, int> Counters = [];

    public static Face GetFont(TextData font)
    {
        if (Fonts.TryGetValue(font.Name, out var face))
        {
            StartUsing(font);
            return face;
        }
        
        return GetFromPath(font);
    }

    public static void Free(TextData font)
    {
        if (!Counters.TryGetValue(font.Name, out var count))
            return;
        
        count = --Counters[font.Name];

        if (count <= 0)
        {
            Counters.Remove(font.Name);
            Fonts.Remove(font.Name);
        }
    }

    private static void StartUsing(TextData font)
    {
        if (!Counters.TryGetValue(font.Name, out var count))
            Counters.Add(font.Name, 1);

        Counters[font.Name]++;
    }

    private static unsafe Face GetFromPath(TextData font)
    {
        var library = new Library();
        Face face;
        try
        {   
            face = library.CreateFace(font.Path);
        }
        catch(FreeTypeException e)
        {
            throw new ResourceException($"Failed to load font {font.Name} on path: {font.Path}.", e);
        }
        Fonts.Add(font.Name, face);
        StartUsing(font);
        return face;
    }

}
