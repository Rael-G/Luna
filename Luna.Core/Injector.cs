namespace Luna.Core;

public class Injector
{
    private static readonly Dictionary<Type, object> Services = [];

    public static void Add<T>(object obj)
    {
        Services.Add(typeof(T), obj);
    }

    public static T? Get<T>()
    {
        Services.GetValueOrDefault(typeof(T));
        return (T?)Services.GetValueOrDefault(typeof(T));
    }
}
