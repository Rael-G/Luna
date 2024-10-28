using System.Collections.Concurrent;
using System.Numerics;

namespace Luna.OpenGL;

public class LightEmitter : ILightEmitter
{
    public static readonly ConcurrentDictionary<string, DirectionalLight> DirLights = [];
    public static readonly ConcurrentDictionary<string, PointLight> PointLights = [];
    public static readonly ConcurrentDictionary<string, SpotLight> SpotLights = [];
    public static readonly List<(Matrix4x4, Texture2D)> ShadowMaps = [];

    //Temporary limit for the actual improvised light system
    private const int DirectionalMaxLenght = 1;
    private const int PointMaxLenght = 10;
    private const int SpotMaxLenght = 10;

    public void Add<T>(string id, T light) where T : DirectionalLight
    {
        if (light is SpotLight spotLight)
        {
            if (SpotLights.Count >= SpotMaxLenght)
                return;
            SpotLights[id] = spotLight;
            return;
        }

        if (light is PointLight pointLight)
        {
            if (PointLights.Count >= PointMaxLenght)
                return;
            PointLights[id] = pointLight;
            return;
        }

        if (DirLights.Count >= DirectionalMaxLenght)
            return;
        DirLights[id] = light;
    }
    
    public void Remove(string id)
    {
        DirLights.Remove(id, out _);
        PointLights.Remove(id, out _);
        SpotLights.Remove(id, out _);
    }

    public static void ClearShadowMaps()
    {
        ShadowMaps.Clear();
    }
}
