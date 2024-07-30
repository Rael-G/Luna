namespace Luna.Core;

public static class Tree
{
    // Is registered by host
    public static Node Root { get; internal set; } = null!;

    private static readonly Dictionary<string, Node> _nodes = [];

    internal static void AddNode(Node node)
        => _nodes[node.UID] = node;

    internal static void RemoveNode(string uid)
        => _nodes.Remove(uid);

    public static T? FindNodeByUID<T>(string uid) where T : Node
        => _nodes.GetValueOrDefault(uid) as T;
}
