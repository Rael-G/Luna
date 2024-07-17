namespace Luna;

public class Node
{
    public string RUID { get; }

    protected virtual Node? Parent { get; set;}

    private List<Node> Children { get; }

    protected bool IsAwake { get; set; }

    public Node()
    {
        RUID = Guid.NewGuid().ToString();
        Children = [];
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
        IsAwake = true;

        foreach (var child in Children)
            child.Awake();
    }

    /// <summary>
    /// Initializes this GameObject.
    /// </summary>
    public virtual void Start()
    { 
        foreach (var child in Children)
            child.Start();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void EarlyUpdate()
    { 
        foreach (var child in Children)
            child.EarlyUpdate();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void Update()
    { 
        foreach (var child in Children)
            child.Update();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    public virtual void LateUpdate()
    {
        foreach (var child in Children)
            child.LateUpdate();
    }

    /// <summary>
    ///  Performs drawing operations.
    /// </summary>
    public virtual void Render()
    {
        var map = Injector.Get<IRenderMap>();
        map.Render(RUID);

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
            node.Parent = this;
            Children.Add(node);
            if (Window.Running && !IsAwake)
            {
                node.Awake();
                node.Start();
            }
            
        }
    }

    public virtual void AddChild(IEnumerable<Node> nodes)
    {
        Children.AddRange(nodes);
        foreach (var node in nodes)
        {
            node.Parent = this;
            if (Window.Running && !IsAwake)
            {
                node.Awake();
                node.Start();
            }
        }
    }

    public virtual void RemoveChild(Node node)
    {
        Children.Remove(node);
        node.Parent = null;
    }

    protected internal void CreateRenderObject<TData>(TData data)
    {
        var factory = Injector.Get<IRenderObjectFactory>();
        var map = Injector.Get<IRenderMap>();

        var obj = factory.CreateRenderObject(data);
        map.Add(RUID, obj);
    }

    protected internal void UpdateRenderObject<TData>(TData data)
    {
        var map = Injector.Get<IRenderMap>();
        map.Update(RUID, data);
    }

    ~Node()
    {
        Children.Clear();
        var renderMap = Injector.Get<IRenderMap>();
        renderMap.Remove(RUID);
    }
}
