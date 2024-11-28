using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class TextureManager
{   
    private static readonly Dictionary<string, Texture> Textures = [];
    private static readonly Dictionary<string, int> Counters = [];

    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    public static Texture2DGL Load(Texture2D texture2D)
    {
        Texture2DGL? texture = Get(texture2D.Hash) as Texture2DGL;
        if (texture is not null)
        {
            StartUsing(texture2D.Hash);
            return texture;
        }

        texture = Create(texture2D);
        Cache(texture2D.Hash, texture);
        return texture;
    }

    public static CubeMapGL Load(CubeMap cubemap)
    {
        CubeMapGL? texture = Get(cubemap.Hash) as CubeMapGL;
        if (texture is not null)
        {
            StartUsing(cubemap.Hash);
            return texture;
        }

        texture = Create(cubemap);
        Cache(cubemap.Hash, texture);
        return texture;
    }

    public static Texture? Get(string hash)
    {
        if (Textures.TryGetValue(hash, out var texture))
            return texture;
        
        return null;
    }

    public static void Dispose(string hash)
    {
        if (--Counters[hash] <= 0)
        {
            Delete(hash);
        }
    }

    public static void Cache(string hash, Texture texture)
    {
        if (Counters.TryGetValue(hash, out _) || Textures.TryGetValue(hash, out _))
            throw new LunaException("Caching an already registered texture is not allowed.");
        
        Textures[hash] = texture;
        Counters[hash] = 0;
        StartUsing(hash);
    }

    public static void Delete(string hash)
    {
        Textures[hash].Dispose();
        Counters.Remove(hash);
        Textures.Remove(hash);
    }

    private static void StartUsing(string hash)
    {
        Counters[hash]++;
    }

    private static Texture2DGL Create(Texture2D texture2D)
    {
        if (!string.IsNullOrEmpty(texture2D.Path))
        {
            return new Texture2DGL(_gl, texture2D.Path, texture2D.FlipV, texture2D.ImageType, 
                texture2D.FilterMode, texture2D.WrapMode);
        }
        else
        {
            return new Texture2DGL(_gl, texture2D.Size, texture2D.ImageType, texture2D.FilterMode, 
                texture2D.WrapMode);
        }
    }

    private static CubeMapGL Create(CubeMap cubeMap)
    {
        if (cubeMap.Paths.Length > 0)
        {
            return new CubeMapGL(_gl, cubeMap.Paths, cubeMap.FlipV, cubeMap.ImageType, cubeMap.FilterMode, 
                cubeMap.WrapMode);
        }
        else
        {
            return new CubeMapGL(_gl, cubeMap.Size, cubeMap.ImageType, cubeMap.FilterMode, cubeMap.WrapMode);
        }
    }

}
