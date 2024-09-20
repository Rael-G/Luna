namespace Luna.OpenGL;

public class TextureManager
{   
    private static readonly Dictionary<string, TextureBase> Textures = [];
    private static readonly Dictionary<string, int> Counters = [];

    internal static GlTexture2D Load(Texture2D texture2D)
    {
        StartUsing(texture2D.Hash);
        GlTexture2D? texture = Get(texture2D.Hash) as GlTexture2D;
        if (texture is not null)
        {
            return texture;
        }

        texture = GlTexture2D.Create(texture2D);
        Cache(texture2D.Hash, texture);
        return texture;
    }

    internal static GlCubeMap Load(CubeMap cubemap)
    {
        StartUsing(cubemap.Hash);
        GlCubeMap? texture = Get(cubemap.Hash) as GlCubeMap;
        if (texture is not null)
        {
            return texture;
        }

        texture = GlCubeMap.Create(cubemap);
        Cache(cubemap.Hash, texture);
        return texture;
    }

    internal static TextureBase? Get(string hash)
    {
        if (Textures.TryGetValue(hash, out var texture))
            return texture;
        
        return null;
    }

    internal static int StopUsing(string hash)
    {
        if (!Counters.TryGetValue(hash, out _))
            return 0;

        return --Counters[hash];
    }

    internal static void Cache(string hash, TextureBase texture)
    {
        Textures[hash] = texture;
    }

    internal static void Dispose(string hash)
        => Get(hash)?.Dispose();

    internal static void Delete(string hash)
    {
        Counters.Remove(hash);
        Textures.Remove(hash);
    }

    private static void StartUsing(string hash)
    {
        if (!Counters.TryGetValue(hash, out _))
            Counters.Add(hash, 0);

        Counters[hash]++;
    }

}
