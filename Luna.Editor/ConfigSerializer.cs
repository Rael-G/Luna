using System.Numerics;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Luna.Editor;

public class ConfigSerializer
{
    public static LunaConfig? ReadConfig(string filePath)
    {
        string yaml = string.Empty;

        try
        {
            yaml = File.ReadAllText(filePath);
        }
        catch(FileNotFoundException)
        {
            return null;
        }

        return DeserializeConfig(yaml);
    }

    public static void WriteConfig(string filePath, LunaConfig config)
    {
        var yaml = SerializeConfig(config);
        File.WriteAllText(filePath, yaml);
    }

    private static string SerializeConfig(LunaConfig config)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return serializer.Serialize(config);
    }

    private static LunaConfig DeserializeConfig(string yaml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<LunaConfig>(yaml);
    }
}

    public class LunaConfig
    {
        public string Title { get; set; } = string.Empty;
        public Vector2 Resolution { get; set; } = new(800f, 600f);
        public bool Fullscreen { get; set; }
        public bool Vsync { get; set; }
    }
