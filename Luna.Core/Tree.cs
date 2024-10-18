namespace Luna;

/// <summary>
/// The Hierarchy of the program
/// </summary>
public static class Tree
{
    public static Node Root 
    {
         get => _root;
         internal set
         {
            _root = value;
            AddNode(value);
         }
    }

    // Is registered by host
    private static Node _root = null!;

    private static readonly Dictionary<string, Node> _nodes = [];

    internal static void AddNode(Node node)
    {
        _nodes[node.UID] = node;
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
        _nodes.Remove(uid);
    }

    public static T? FindNodeByUID<T>(string uid) where T : Node
        => _nodes.GetValueOrDefault(uid) as T;
}
