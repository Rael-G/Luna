namespace Luna.OpenGL;

internal class Renderer : IRenderer
{
    private static readonly Dictionary<string, IRenderObject> _renderer = [];

    private static readonly SortedDictionary<int, Queue<IRenderObject>> _priorityDictionary = [];

    private static bool _interrupt;

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
            if (!_priorityDictionary.TryGetValue(renderObject.Priority, out var queue))
            {
                queue = [];
                _priorityDictionary[renderObject.Priority] = queue;
            }
            queue.Enqueue(renderObject);
        }
    }

    public void Update<TData>(string id, TData tData)
    {
        var renderObject = _renderer.GetValueOrDefault(id) as RenderObject<TData>;
        renderObject?.Update(tData);
    }

    public void DrawQueue(IMaterial? material = null, bool clear = true)
    {
        foreach (var (key, queue) in _priorityDictionary.Reverse())
        {
            foreach(var node in queue)
            {
                if (_interrupt)
                    break;

                if (material is null)
                {
                    node.Draw();
                    continue;
                }

                node.Draw(material);
            }
        }

        if (clear)
        {
            _priorityDictionary.Clear();
        }

        _interrupt = false;
    }

    public void ClearRoutine()
    {
        LightEmitter.ClearShadowMaps();
    }

    internal static void Interrupt()
    {
        _interrupt = true;
    }
}
