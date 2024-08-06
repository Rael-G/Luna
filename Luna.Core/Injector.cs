namespace Luna;

public static class Injector
{
    private enum ServiceLifetime
    {
        Singleton,
        Transient
    }

    private class ServiceDescriptor
    {
        public Func<object> Factory { get; }
        public ServiceLifetime Lifetime { get; }
        private Lazy<object>? SingletonInstance { get; set; }

        public ServiceDescriptor(Func<object> factory, ServiceLifetime lifetime)
        {
            Factory = factory;
            Lifetime = lifetime;
            
            if (lifetime == ServiceLifetime.Singleton)
            {
                SingletonInstance = new Lazy<object>(factory);
            }
        }

        public object GetService()
        {
            return Lifetime == ServiceLifetime.Singleton ? SingletonInstance!.Value : Factory();
        }
    }

    private static readonly Dictionary<Type, ServiceDescriptor> Services = [];

    /// <summary>
    /// Registers a singleton service.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="obj">The instance of the singleton service.</param>
    /// <exception cref="DependencyInjectionException"></exception>
    public static void AddSingleton<T>(T obj) where T : class
        => Services[typeof(T)] = new ServiceDescriptor(() => obj, ServiceLifetime.Singleton);
    
    /// <summary>
    /// Registers a transient service with a factory method.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="factory">The factory method that creates a new instance of the service.</param>
    /// <exception cref="DependencyInjectionException"></exception>
    public static void AddTransient<T>(Func<T> factory) where T : class
        => Services[typeof(T)] = new ServiceDescriptor(() => factory(), ServiceLifetime.Transient);
    

    /// <summary>
    /// Gets a new instance of the requested service type.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <returns>A new instance of the requested service.</returns>
    /// <exception cref="DependencyInjectionException"></exception>
    public static T Get<T>()
    {
        if (Services.TryGetValue(typeof(T), out var descriptor))
        {
            return (T)descriptor.GetService();
        }
        throw new DependencyInjectionException($"Service of type {typeof(T).Name} is not registered.");
    }
}