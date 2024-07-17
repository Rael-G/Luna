namespace Luna;

public class Injector
{
    private static readonly Dictionary<Type, Lazy<object>> Services = [];

    /// <summary>
    /// Injector.AddSingleton<IMyService>(new MyService());
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void AddSingleton<T>(object obj) where T : class
    {
        if (!Services.TryAdd(typeof(T), new Lazy<object>(() => obj)))
        {
            throw new DependencyInjectionException($"Service of type {typeof(T).Name} is already registered.");
        }
    }

    /// <summary>
    /// Injector.AddTransient<IMyService>(() => new MyService());
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void AddTransient<T>(Func<T> factory) where T : class
    {
        if (!Services.TryAdd(typeof(T), new Lazy<object>(() => factory())))
        {
            throw new DependencyInjectionException($"Service of type {typeof(T).Name} is already registered.");
        }
    }

    public static T Get<T>()
    {
        if (Services.TryGetValue(typeof(T), out var service))
        {
            return (T)service.Value;
        }
        throw new DependencyInjectionException($"Service of type {typeof(T).Name} is not registered.");
    }
}
