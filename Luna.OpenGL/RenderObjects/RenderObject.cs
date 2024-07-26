using Luna.Core;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public abstract class RenderObject<TData>() 
    : Disposable, IRenderObject
{
    protected static readonly GL GL = Window.GL ?? throw new WindowException("Window.GL is null.");
        
    public abstract void Render();

    public abstract void Update(TData data);
}
