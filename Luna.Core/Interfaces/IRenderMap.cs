namespace Luna.Core;

public interface IRenderMap
{
    void Add(string id, IRenderObject renderObject);
    void Remove(string id);
    void Render(string id);
    public void Update<TData>(string id, TData tData);
}