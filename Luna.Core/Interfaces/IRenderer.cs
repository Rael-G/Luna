namespace Luna;

public interface IRenderer
{
    void Add(string id, IRenderObject renderObject);
    void Remove(string id);
    void Draw(string id);
    public void Update<TData>(string id, TData tData);
}