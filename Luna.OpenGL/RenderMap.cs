namespace Luna.OpenGl;

internal class RenderMap : IRenderMap
{
    private readonly Dictionary<string, IRenderObject> _renderMap = [];

    public void Add(string id, IRenderObject renderObject)
        => _renderMap.Add(id, renderObject);
    
    public void Remove(string id)
    {
        _renderMap.GetValueOrDefault(id)?.Dispose();
        _renderMap.Remove(id);
    }
    
    public void Render(string id)
    {
        _renderMap.TryGetValue(id, out IRenderObject? renderObject);
        renderObject?.Render();
    }

    public void Update<TData>(string id, TData tData)
    {
        var renderObject = _renderMap.GetValueOrDefault(id) as RenderObject<TData>;
        renderObject?.Update(tData);
    }
}
