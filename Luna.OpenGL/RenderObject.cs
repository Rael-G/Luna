namespace Luna.OpenGl;

public abstract class RenderObject<TData>() 
    : IRenderObject
{
    public abstract void Render();

    public abstract void Update(TData data);
}
