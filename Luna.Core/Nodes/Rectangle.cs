using Luna.Maths;

namespace Luna.Core;

public class Rectangle : Node2D
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Color Color { get; set; }

    internal override void InternalAwake()
    {
        var ViewProj = Camera?.Project()?? Matrix.Identity(4);
        var tranform = ViewProj * Transform.ModelMatrix(); 
        var renderMap = Injector.Get<IRenderMap>();
        var factory = Injector.Get<IRenderObjectFactory>();
        renderMap.Add(RUID, factory!.CreateRenderObject(new RectangleData
        { 
            Width = Width, Height = Height, Transform = tranform, Color = Color 
        }));

        base.InternalAwake();
    }

    internal protected override void Render()
    {
        var ViewProj = Camera?.Project()?? Matrix.Identity(4);
        var tranform = ViewProj * Transform.ModelMatrix(); 
        var renderMap = Injector.Get<IRenderMap>();
        renderMap.Update(RUID, new RectangleData
        { 
            Width = Width, Height = Height, Transform = tranform, Color = Color 
        });
        renderMap.Render(RUID);

        base.Render();
    }

    ~Rectangle()
    {
        var renderMap = Injector.Get<IRenderMap>();
        renderMap.Remove(RUID);
    }

}
