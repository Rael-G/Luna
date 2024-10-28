using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public abstract class RenderObject<TData>() 
    : Disposable, IRenderObject
{
    protected static readonly GL GL = Window.GL ?? throw new WindowException("Window.GL is null.");

    public virtual int Priority { get; protected set; }
        
    public abstract void Draw();

    public abstract void Draw(IMaterial material);

    public abstract void Update(TData data);
}
