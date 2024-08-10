namespace Luna.OpenGL;

internal class Renderer : IRenderer
{
    private readonly Dictionary<string, IRenderObject> _renderer = [];

    public void Add(string id, IRenderObject renderObject)
        => _renderer.Add(id, renderObject);
    
    public void Remove(string id)
    {
        _renderer.GetValueOrDefault(id)?.Dispose();
        _renderer.Remove(id);
    }
    
    public void Draw(string id)
    {
        _renderer.TryGetValue(id, out IRenderObject? renderObject);
        renderObject?.Draw();
    }

    public void Update<TData>(string id, TData tData)
    {
        var renderObject = _renderer.GetValueOrDefault(id) as RenderObject<TData>;
        renderObject?.Update(tData);
    }
}
