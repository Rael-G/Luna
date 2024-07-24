namespace Luna;

public interface IRenderObjectFactory
{
    IRenderObject Create<TData>(TData data);
}
