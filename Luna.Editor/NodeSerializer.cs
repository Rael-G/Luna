using System.Reflection;
using System.Runtime.Serialization;

namespace Luna.Editor;

public static class NodeSerializer
{
    public static void SaveToFile<T>(T node, string filePath, Assembly assembly) where T : Node
    {
        var types = GetAllSerializableTypes(assembly);

        var serializer = new DataContractSerializer(node.GetType(), types);
        using var fs = new FileStream(filePath, FileMode.OpenOrCreate);
        serializer.WriteObject(fs, node);
    }

    public static object? LoadFromFile(string filePath, Type type, Assembly assembly)
    {
        if (!typeof(Node).IsAssignableFrom(type))
        {
            throw new LunaException("Type loaded must be assignable from Node.");
        }

        var serializer = new DataContractSerializer(type, GetAllSerializableTypes(assembly));
        using var fs = new FileStream(filePath, FileMode.Open);
        var root = serializer.ReadObject(fs);
        
        return root;
    }

    private static List<Type> GetAllSerializableTypes(Assembly assembly)
    {
        var types = GetSerializableTypesFromAssembly(assembly);

        foreach (var referenceName in assembly.GetReferencedAssemblies())
        {
           var referenceAssembly = Assembly.Load(referenceName);

           foreach (var type in GetSerializableTypesFromAssembly(referenceAssembly))
           {
                types.Add(type);
           }
        }

        return [.. types];
    }

    private static HashSet<Type> GetSerializableTypesFromAssembly(Assembly assembly)
    {
        HashSet<Type> types = [];
        try
        {
            var derivedTypes = assembly.GetTypes().Where(t => t.IsDefined(typeof(SerializeAttribute)));

            foreach (var type in derivedTypes)
            {
                types.Add(type);
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            foreach (var type in ex.Types.Where(t => t != null))
            {
                types.Add(type);
            }
        }

        return types;
    }
}
