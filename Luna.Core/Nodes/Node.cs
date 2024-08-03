using Luna.Maths;

namespace Luna;

public class Node : Disposable
{
    public string UID { get; }

    public string Alias { get; set; } 

    public bool Paused { get; set; }

    public bool Invisible { get; set; }

    public virtual Transform Transform { get; }

    protected virtual Node? Parent 
    { 
        get => _parent;
        set 
        {
            _parent = value;
            Transform.Parent = _parent?.Transform;
        }
    }

    internal virtual ICamera? Camera 
    { 
        get
        {
            if (_camera is not null)
                return _camera;

            return _parent?.Camera;
        }
        set
        {
            _camera = value;
        } 
    }

    public ModelViewProjection ModelViewProjection
    {
        get => new()
        {
            Projection = Camera?.Projection?? Matrix.Identity(4),
            View = Camera?.View?? Matrix.Identity(4),
            Model = Transform.ModelMatrix()
        };
    }

    private List<Node> _children;
    private bool _awakened;
    private bool _started;
    private Node? _parent;
    private ICamera? _camera;

    public Node()
    {
        UID = Guid.NewGuid().ToString();
        _children = [];
        Alias = GetType().Name;
        Transform = new();
    }

    /// <summary>
    ///  Configures this GameObject only before window initialization.
    /// </summary>
    public virtual void Config()
    {
        
    }

    internal void InternalConfig()
    {
        Config();
        foreach (var child in _children)
            child.InternalConfig();
    }

    /// <summary>
    /// Performs initialization operations before the Start method.
    /// </summary>
    public virtual void Awake()
    {

    }

    internal void InternalAwake()
    {
        if (_awakened) return;
        Awake();
        foreach (var child in _children)
            child.InternalAwake();
        _awakened = true;
    }

    /// <summary>
    /// Initializes this GameObject.
    /// </summary>
    public virtual void Start()
    {

    }

    internal void InternalStart()
    {
        if (_started) return;
        Start();
        foreach (var child in _children)
            child.InternalStart();
        _started = true;
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void EarlyUpdate()
    {   

    }

    internal void InternalEarlyUpdate()
    {   
        if (Paused)  return;

        EarlyUpdate();
        foreach (var child in _children)
            child.InternalEarlyUpdate();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void Update()
    {

    }

    internal void InternalUpdate()
    {   
        if (Paused)  return;

        Update();
        foreach (var child in _children)
            child.InternalUpdate();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void LateUpdate()
    {

    }

    internal void InternalLateUpdate()
    {   
        if (Paused)  return;

        LateUpdate();
        foreach (var child in _children)
            child.InternalLateUpdate();
    }

    public virtual void FixedUpdate()
    {

    }

    internal void InternalFixedUpdate()
    {   
        if (Paused)  return;

        FixedUpdate();
        foreach (var child in _children)
            child.InternalFixedUpdate();
    }

    /// <summary>
    ///  Performs drawing operations.
    /// </summary>
    internal virtual void Render()
    {
        if (Invisible)  return;

        var map = Injector.Get<IRenderMap>();
        map.Render(UID);

        foreach (var child in _children)
            child.Render();
    }

    public virtual void Input(InputEvent inputEvent)
    {
        foreach (var child in _children)
            child.Input(inputEvent);
    }

    public virtual void AddChild(params Node[] nodes)
    {
        foreach (var node in nodes)
        {
            if (_children.FirstOrDefault(c => c == node) != null) 
                continue;

            node.Parent?.RemoveChild(node);

            node.Parent = this;
            _children.Add(node);
            if (Window.Running && _awakened)   
                node.Awake();
            if (Window.Running && _started) 
                node.Start();
            Tree.AddNode(node);
        }
    }

    public virtual void RemoveChild(Node node)
    {
        _children.Remove(node);
        node.Parent = null;
        Tree.RemoveNode(node.UID);
        node.Dispose();
    }

    // Alias is opcional, if you want to use this method to find a node, every node on the hierarchy path need to have an alias
    public T? FindNode<T>(string alias) where T : Node
    {
        var aliases = alias.Split('/', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        Node? node = this;
        for (int i = 0; i < aliases.Length; i++)
        {
            if (node is null) return null;
            node = node?._children.Find(n => n.Alias == aliases[i]);
        }

        return node as T;
    }

    protected internal void CreateRenderObject<TData>(TData data)
    {
        var factory = Injector.Get<IRenderObjectFactory>();
        var map = Injector.Get<IRenderMap>();

        var obj = factory.Create(data);
        map.Add(UID, obj);
    }

    protected internal void UpdateRenderObject<TData>(TData data)
    {
        var map = Injector.Get<IRenderMap>();
        map.Update(UID, data);
    }

    public override void Dispose(bool disposing)
    {
        var renderMap = Injector.Get<IRenderMap>();
        renderMap.Remove(UID);

        foreach (var child in _children)
        {
            child.Dispose();
        }
    }
}
