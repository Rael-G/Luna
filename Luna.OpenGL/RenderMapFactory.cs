namespace Luna.OpenGl;

public delegate IRenderObject AddRenderObject(object data);

internal class RenderObjectFactory : IRenderObjectFactory
{
    private static readonly Dictionary<Type, AddRenderObject> CreationCallbacks = [];

    public static void RegisterCallback<TData>(Func<TData, IRenderObject> callback)
    {
        CreationCallbacks[typeof(TData)] = data => callback((TData)data);
    }

    public IRenderObject CreateRenderObject<TData>(TData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        if (CreationCallbacks.TryGetValue(typeof(TData), out var callback))
        {
            return callback(data);
        }

        throw new DependencyInjectionException($"CreateRenderObject for type {typeof(TData).Name} is not possible or has not been implemented.");
    }
}
