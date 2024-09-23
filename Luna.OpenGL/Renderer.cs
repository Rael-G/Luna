namespace Luna.OpenGL;

internal class Renderer : IRenderer
{
    private readonly Dictionary<string, IRenderObject> _renderer = [];

    private readonly Queue<IRenderObject> _queue = [];

    public void Add(string id, IRenderObject renderObject)
        => _renderer.Add(id, renderObject);
    
    public void Remove(string id)
    {
        _renderer.GetValueOrDefault(id)?.Dispose();
        _renderer.Remove(id);
    }
    
    public void Enqueue(string id)
    {
        if (_renderer.TryGetValue(id, out IRenderObject? renderObject))
        {
            _queue.Enqueue(renderObject);
        }
    }

    public void Update<TData>(string id, TData tData)
    {
        var renderObject = _renderer.GetValueOrDefault(id) as RenderObject<TData>;
        renderObject?.Update(tData);
    }

    public void DrawQueue(bool clear = true)
    {
        foreach (var node in _queue)
        {
            node.Draw();
        }

        if (clear)
        {
            _queue.Clear();
        }
    }

    public void Draw(string uid)
    {
        if (_renderer.TryGetValue(uid, out IRenderObject? renderObject))
        {
            renderObject.Draw();
        }
    }
    
}
