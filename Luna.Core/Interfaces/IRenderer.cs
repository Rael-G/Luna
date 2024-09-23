namespace Luna;

public interface IRenderer
{
    void Add(string id, IRenderObject renderObject);
    void Remove(string id);
    void Enqueue(string id);
    void Update<TData>(string id, TData tData);
    void DrawQueue(bool clear = true);
    void Draw(string uid);
}