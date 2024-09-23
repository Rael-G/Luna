using System.Xml.Serialization;

namespace Luna.Editor;

public static class NodeSerializer
{
    public static void SaveToFile<T>(T node, string filePath) where T : Node
    {
        XmlSerializer serializer = new(typeof(T));
        using StreamWriter writer = new(filePath);
        serializer.Serialize(writer, node);
    }

    public static T? LoadFromFile<T>(string filePath) where T : Node
    {
        XmlSerializer serializer = new(typeof(Node));
        using StreamReader reader = new(filePath);
        return serializer.Deserialize(reader) as T;
    }
}
