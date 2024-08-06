namespace Luna;

public interface ILightEmitter
{
    void Add<T>(string id, T light) where T : DirectionalLight;

    void Remove(string id);
}
