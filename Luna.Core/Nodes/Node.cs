using System.Numerics;
using System.Runtime.Serialization;

namespace Luna;

[Serialize]
public class Node : Disposable
{
    [IgnoreDataMember]
    public string UID { get; }

    public string Alias { get; set; } 

    public bool Paused { get; set; }

    public bool Invisible { get; set; }

    public virtual Transform Transform { get; set; }

    public List<Node> Children 
    { 
        get => _children;
        set
        {
            foreach(var node in _children)
            {
                RemoveChild(node);
            }

            foreach(var node in value)
            {
                AddChild(node);
            }
        }
    }

    public Node? Parent 
    { 
        get => _parent;
        private set 
        {
            _parent = value;
            Transform.Parent = _parent?.Transform;
        }
    }

    /// <summary>
    /// Gets or sets the camera associated with this node. 
    /// 
    /// When getting the camera, if this node has a camera assigned, it returns that camera. 
    /// If no camera is assigned to this node, it recursively checks the parent nodes for an assigned camera. 
    /// If no cameras are found in the hierarchy, it returns null.
    /// 
    /// When setting a camera, it directly assigns the specified camera to this node, 
    /// without affecting parent or child nodes.
    /// </summary>
    public ICamera? Camera 
    { 
        get
        {
            if (_camera is not null) return _camera;

            return _parent?.Camera;
        }
        set
        {
            _camera = value;
        } 
    }

    protected ModelViewProjection ModelViewProjection
    {
        get => new()
        {
            Projection = Camera?.Projection?? Matrix4x4.Identity,
            View = Camera?.View?? Matrix4x4.Identity,
            Model = Transform.ModelMatrix()
        };
    }

    [IgnoreDataMember]
    public Action? AwakeAction { get; set; }

    [IgnoreDataMember]
    public Action? StartAction { get; set; }

    [IgnoreDataMember]
    public Action? EarlyUpdateAction { get; set; }

    [IgnoreDataMember]
    public Action? UpdateAction { get; set; }

    [IgnoreDataMember]
    public Action? LateUpdateAction { get; set; }

    [IgnoreDataMember]
    public Action? FixedUpdateAction { get; set; }

    private bool _awakened;
    private bool _started;
    private Node? _parent;
    private ICamera? _camera;
    private readonly List<Node> _children = [];

    public Node()
    {
        UID = Guid.NewGuid().ToString();
        Children = [];
        Alias = GetType().Name;
        Transform = new();
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
        AwakeAction?.Invoke();
        foreach (var child in Children)
            child.InternalAwake();
        _awakened = true;
    }

    /// <summary>
    /// Performs initialization operations.
    /// </summary>
    public virtual void Start()
    {

    }

    internal void InternalStart()
    {
        if (_started) return;
        Start();
        StartAction?.Invoke();
        foreach (var child in Children)
            child.InternalStart();
        _started = true;
    }

    /// <summary>
    /// Updates this Node once per frame early.
    /// </summary>
    public virtual void EarlyUpdate()
    {   

    }

    internal void InternalEarlyUpdate()
    {   
        if (Paused)  return;

        EarlyUpdate();
        EarlyUpdateAction?.Invoke();
        foreach (var child in Children)
            child.InternalEarlyUpdate();
    }

    /// <summary>
    /// Updates this Node once per frame.
    /// </summary>
    public virtual void Update()
    {

    }

    internal void InternalUpdate()
    {   
        if (Paused)  return;

        Update();
        UpdateAction?.Invoke();
        foreach (var child in Children)
            child.InternalUpdate();
    }

    /// <summary>
    /// Updates this Node once per frame late.
    /// </summary>
    public virtual void LateUpdate()
    {

    }

    internal void InternalLateUpdate()
    {   
        if (Paused)  return;

        LateUpdate();
        LateUpdateAction?.Invoke();
        foreach (var child in Children)
            child.InternalLateUpdate();
    }

    /// <summary>
    /// Updates this Node at a fixed time.
    /// </summary>
    public virtual void FixedUpdate()
    {

    }

    internal void InternalFixedUpdate()
    {   
        if (Paused)  return;

        FixedUpdate();
        FixedUpdateAction?.Invoke();
        foreach (var child in Children)
            child.InternalFixedUpdate();
    }

    /// <summary>
    /// Performs drawing operations.
    /// </summary>
    internal virtual void Draw()
    {
        if (Invisible)  return;

        var map = Injector.Get<IRenderer>();
        map.Enqueue(UID);

        foreach (var child in Children)
            child.Draw();
    }

    /// <summary>
    /// Is called when an input event occurs.
    /// </summary>
    /// <param name="inputEvent"></param>
    public virtual void Input(InputEvent inputEvent)
    {
        foreach (var child in Children)
            child.Input(inputEvent);
    }

    /// <summary>
    /// Make nodes children of this one and call its Awake and Start methods if necessary.
    /// </summary>
    /// <param name="nodes"></param>
    public virtual void AddChild(params Node[] nodes)
    {
        foreach (var node in nodes)
        {
            if (Children.FirstOrDefault(c => c == node) != null) 
                continue;

            node.Parent?.RemoveChild(node);

            node.Parent = this;
            Children.Add(node);
            if (Window.Running && _awakened)   
                node.InternalAwake();
            if (Window.Running && _started) 
                node.InternalStart();
            Tree.AddNode(node);
        }
    }

    /// <summary>
    /// Disaffiliates nodes and removes them from the tree.
    /// </summary>
    /// <param name="node"></param>
    public virtual void RemoveChild(params Node[] nodes)
    {
        foreach(var node in nodes)
        {
            Children.Remove(node);
            node.Parent = null;
            Tree.RemoveNode(node.UID);
            node.Dispose();
        }
    }

    /// <summary>
    /// Find a node among its descendents. e.g. Node tongue = Body.FindNode("Head/Mouth/Tongue")
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="alias"></param>
    /// <returns></returns>
    public Node? FindNode(string alias)
    {
        var aliases = alias.Split('/', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        Node? node = this;
        for (int i = 0; i < aliases.Length; i++)
        {
            if (node is null) return null;
            node = node?.Children.Find(n => n.Alias == aliases[i]);
        }

        return node;
    }

    /// <summary>
    /// Find a node of the generic type among its descendents. e.g. Tongue tongue = Body.FindNode<Tongue>("Head/Mouth/Tongue")
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="alias"></param>
    /// <returns></returns>
    public T? FindNode<T>(string alias) where T : Node
        => FindNode(alias) as T;

    protected internal void CreateRenderObject<TData>(TData data)
    {
        var factory = Injector.Get<IRenderObjectFactory>();
        var map = Injector.Get<IRenderer>();

        var obj = factory.Create(data);
        map.Add(UID, obj);
    }

    protected internal void UpdateRenderObject<TData>(TData data)
    {
        var map = Injector.Get<IRenderer>();
        map.Update(UID, data);
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        Injector.Get<IRenderer>().Remove(UID);

        foreach (var child in Children)
            child.Dispose();
        
        base.Dispose(disposing);
    }
}
