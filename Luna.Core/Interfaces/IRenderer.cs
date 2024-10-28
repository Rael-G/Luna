namespace Luna;

public interface IRenderer
{
    void Add(string id, IRenderObject renderObject);
    void Remove(string id);
    void Enqueue(string id);
    void Update<TData>(string id, TData tData);
    void DrawQueue(IMaterial? material = null, bool clear = true);
    void ClearRoutine();
}