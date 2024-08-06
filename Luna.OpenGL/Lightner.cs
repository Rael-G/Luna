using System.Collections.Concurrent;

namespace Luna.OpenGL;

public class LightEmitter : ILightEmitter
{
    public static readonly ConcurrentDictionary<string, DirectionalLight> DirLights = [];
    public static readonly ConcurrentDictionary<string, PointLight> PointLights = [];
    public static readonly ConcurrentDictionary<string, SpotLight> SpotLights = [];

    //Temporary limit for the actual improvised light system
    private const int DirectionalMaxLenght = 1;
    private const int PointMaxLenght = 10;
    private const int SpotMaxLenght = 10;

    public void Add<T>(string id, T light) where T : DirectionalLight
    {
        if (light is DirectionalLight dirLight)
        {
            if (DirLights.Count >= DirectionalMaxLenght)
                return;
            DirLights[id] = dirLight;
            return;
        }

        if (light is PointLight pointLight)
        {
            if (PointLights.Count >= PointMaxLenght)
                return;
            PointLights[id] = pointLight;
            return;
        }

        if (light is SpotLight spotLight)
        {
            if (SpotLights.Count >= SpotMaxLenght)
                return;
            SpotLights[id] = spotLight;
            return;
        }
    }
    
    public void Remove(string id)
    {
        DirLights.Remove(id, out _);
        PointLights.Remove(id, out _);
        SpotLights.Remove(id, out _);
    }
}
