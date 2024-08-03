namespace Luna.OpenGL;

public class TextureManager
{   
    private static readonly Dictionary<string, Texture> Textures = [];
    private static readonly Dictionary<string, int> Counters = [];

    public static Texture? GetTexture(string hash)
    {
        if (Textures.TryGetValue(hash, out var texture))
            return texture;
        
        return null;
    }

    public static void StartUsing(string hash)
    {
        if (!Counters.TryGetValue(hash, out _))
            Counters.Add(hash, 0);

        Counters[hash]++;
    }

    public static int StopUsing(string hash)
    {
        if (!Counters.TryGetValue(hash, out _))
            return 0;

        return --Counters[hash];
    }

    public static void Cache(string hash, Texture texture)
    {
        Textures[hash] = texture;
    }

    public static void Delete(string hash)
    {
        Counters.Remove(hash);
        Textures.Remove(hash);
    }

}
