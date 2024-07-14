namespace Luna.Core;

public class Node
{
    internal string RUID { get; }

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
    protected virtual void Config(){ }

    internal virtual void InternalConfig()
    {
        Config();

        foreach (var child in Children)
            child.InternalConfig();
    }

    /// <summary>
    /// Performs initialization operations before the Start method.
    /// </summary>
    protected virtual void Awake(){ }

    internal virtual void InternalAwake()
    {
        Awake();
        IsAwake = true;

        foreach (var child in Children)
            child.InternalAwake();
    }

    /// <summary>
    /// Initializes this GameObject.
    /// </summary>
    protected virtual void Start(){ }

    internal virtual void InternalStart()
    {
        Start();
        foreach (var child in Children)
            child.InternalStart();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    protected virtual void EarlyUpdate(){ }

    internal virtual void InternalEarlyUpdate()
    {
        EarlyUpdate();
        foreach (var child in Children)
            child.InternalEarlyUpdate();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    protected virtual void Update(){ }

    internal virtual void InternalUpdate()
    {
        Update();
        foreach (var child in Children)
            child.InternalUpdate();
    }

    /// <summary>
    /// Updates this GameObject once per frame.
    /// </summary>
    protected virtual void LateUpdate(){ }

    internal virtual void InternalLateUpdate()
    {
        LateUpdate();
        foreach (var child in Children)
            child.InternalLateUpdate();
    }

    /// <summary>
    ///  Performs drawing operations.
    /// </summary>
    internal protected virtual void Render()
    {
        foreach (var child in Children)
            child.Render();
    }

    public virtual void AddChild(params Node[] nodes)
    {
        foreach (var node in nodes)
        {
            node.Parent = this;
            Children.Add(node);
            if (Window.Running && !IsAwake)
            {
                node.InternalAwake();
                node.InternalStart();
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
                node.InternalAwake();
                node.InternalStart();
            }
        }
    }

    public virtual void RemoveChild(Node node)
    {
        Children.Remove(node);
        node.Parent = null;
    }

    ~Node()
    {
        Children.Clear();
    }
}
