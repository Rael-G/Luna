using System.Numerics;
using Luna.Maths;

namespace Luna;

public class Rectangle : Node
{
    public Vector2 Size { get; set; }

    public Color Color { get; set; } = Colors.White;

    public bool Center { get; set; }
    
    public IStandardMaterial Material { get; set; } = Injector.Get<IStandardMaterial>();

    public override void Awake()
    {
        Material.Color = Color;
        Material.ModelViewProjection = ModelViewProjection;
        CreateRenderObject
        (
            
            new RectangleData
            { 
                Size = Size, Material = Material
            }
        );

        base.Awake();
    }

    public override void EarlyUpdate()
    {
        CenterRect();
        base.EarlyUpdate();
    }

    public override void LateUpdate()
    {
        Material.Color = Color;
        Material.ModelViewProjection = ModelViewProjection;
        UpdateRenderObject
         (
            new RectangleData
            { 
                Size = Size, Material = Material
            }
        );
        base.LateUpdate();
    }

    private void CenterRect()
    {
        if (Center)
            Transform.Origin = Size.ToVector3() * Transform.Scale / 2f;
    }
}
