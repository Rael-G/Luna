using Luna.Core;

namespace Luna.Engine.OpenGl;

internal abstract class RenderObject<TData>() 
    : IRenderObject
{
    public abstract void Render();

    public abstract void Update(TData data);
}
