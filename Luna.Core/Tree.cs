namespace Luna;

/// <summary>
/// The Hierarchy of the program
/// </summary>
public static class Tree
{
    public static Node Root
    {
        get => _root ?? throw new LunaException("Root is null.");
        internal set
        {
            if (_root is not null)
            {
                RemoveNode(_root.UID);
            }
            _root = value;
            AddNode(value);
        }
    }

    private static Node? _root;

    private static readonly Dictionary<string, Node> _index = [];

    public static T? FindNodeByUID<T>(string uid) where T : Node
        => FindNodeByUID(uid) as T;

    public static Node? FindNodeByUID(string uid)
        => _index.GetValueOrDefault(uid);

    internal static void AddNode(Node node)
    {
        if (!IsDescendantOfRoot(node)) return;

        _index[node.UID] = node;
        foreach (var child in node.Children)
        {
            AddNode(child);
        }
    }

    internal static void RemoveNode(string uid)
    {
        var node = FindNodeByUID<Node>(uid);

        if (node is null) return;

        foreach (var child in node.Children)
        {
            RemoveNode(child.UID);
        }
        node.Dispose();
        _index.Remove(uid);
    }

    private static bool IsDescendantOfRoot(Node node)
    {
        var current = node;
        while (current != null)
        {
            if (current == Root) return true;
            current = current.Parent;
        }
        return false;
    }
    
    public static IEnumerable<T> GetAllNodesOfType<T>() where T : Node
        => _index.Values.OfType<T>();
    
}
