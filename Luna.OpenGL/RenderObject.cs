using Luna.Core;

namespace Luna.OpenGl;

public abstract class RenderObject<TData>() 
    : Disposable, IRenderObject
{
    public abstract void Render();

    public abstract void Update(TData data);
}
