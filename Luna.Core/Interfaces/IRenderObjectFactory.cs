namespace Luna;

public interface IRenderObjectFactory
{
    IRenderObject CreateRenderObject<TData>(TData data);
}
