using Luna.Core;

namespace Luna;

public class Node : IFixed
{
    public string UID { get; }

    public string Alias { get; set; } 

    public bool Paused { get; set; }

    public bool Invisible { get; set; }

    protected virtual Node? Parent { get; set;}

    private bool _awake;
    private bool _started;

    private List<Node> Children { get; }


    public Node()
    {
        UID = Guid.NewGuid().ToString();
        Children = [];
        Alias = GetType().Name;
    }

    /// <summary>
    ///  Configures this GameObject only before window initialization.
    /// </summary>
    public virtual void Config()
    {
        foreach (var child in Children)
            child.Config();
    }

    /// <summary>
    /// Performs initialization operations before the Start method.
    /// </summary>
    public virtual void Awake()
    {
        if (_awake)    return;

        _awake = true;
        foreach (var child in Children)
            child.Awake();
    }

    /// <summary>
    /// Initializes this GameObject.
    /// </summary>
    public virtual void Start()
    {
        if (_started)  return;

        _started = true;
        foreach (var child in Children)
            child.Start();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void EarlyUpdate()
    {   
        if (Paused)  return;

        foreach (var child in Children)
            child.EarlyUpdate();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void Update()
    {
        if (Paused)  return;

        foreach (var child in Children)
            child.Update();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void LateUpdate()
    {
        if (Paused)  return;

        foreach (var child in Children)
            child.LateUpdate();
    }

    public virtual void FixedUpdate()
    {
        if (Paused)  return;

        foreach (var child in Children)
            child.FixedUpdate();
    }

    /// <summary>
    ///  Performs drawing operations.
    /// </summary>
    public virtual void Render()
    {
        if (Invisible)  return;

        var map = Injector.Get<IRenderMap>();
        map.Render(UID);

        foreach (var child in Children)
            child.Render();
    }

    public virtual void Input(InputEvent inputEvent)
    {
        foreach (var child in Children)
            child.Input(inputEvent);
    }

    public virtual void AddChild(params Node[] nodes)
    {
        foreach (var node in nodes)
        {
            if (Children.FirstOrDefault(c => c == node) != null) 
                continue;

            node.Parent = this;
            Children.Add(node);
            if (Window.Running && _awake)   node.Awake();
            if (Window.Running && _started) node.Start();
            Tree.AddNode(node);
        }
    }

    public virtual void RemoveChild(Node node)
    {
        Children.Remove(node);
        node.Parent = null;
        Tree.RemoveNode(node.UID);
    }

    // Alias is opcional, if you want to use this method to find a node, every node on the hierarchy path need to have an alias
    public T? FindNode<T>(string alias) where T : Node
    {
        var aliases = alias.Split('/', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        Node? node = this;
        for (int i = 0; i < aliases.Length; i++)
        {
            if (node is null) return null;
            node = node?.Children.Find(n => n.Alias == aliases[i]);
        }

        return node as T;
    }

    protected internal void CreateRenderObject<TData>(TData data)
    {
        var factory = Injector.Get<IRenderObjectFactory>();
        var map = Injector.Get<IRenderMap>();

        var obj = factory.CreateRenderObject(data);
        map.Add(UID, obj);
    }

    protected internal void UpdateRenderObject<TData>(TData data)
    {
        var map = Injector.Get<IRenderMap>();
        map.Update(UID, data);
    }

    ~Node()
    {
        var renderMap = Injector.Get<IRenderMap>();
        renderMap.Remove(UID);
    }
}
