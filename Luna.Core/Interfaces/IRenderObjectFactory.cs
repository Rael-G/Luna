namespace Luna.Core;

public interface IRenderObjectFactory
{
    IRenderObject CreateRenderObject<TData>(TData data);
}
