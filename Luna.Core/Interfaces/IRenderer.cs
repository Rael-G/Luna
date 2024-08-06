namespace Luna;

public interface IRenderer
{
    void Add(string id, IRenderObject renderObject);
    void Remove(string id);
    void Render(string id);
    public void Update<TData>(string id, TData tData);
}