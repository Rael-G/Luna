using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public abstract class RenderObject<TData>() 
    : Disposable, IRenderObject
{
    protected static readonly GL GL = Window.GL ?? throw new WindowException("Window.GL is null.");
        
    public abstract void Draw();

    public abstract void Update(TData data);
}
