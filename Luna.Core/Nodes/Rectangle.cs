using System.Numerics;

namespace Luna;

public class Rectangle : Node2D
{
    public Vector2 Size { get; set; }

    public Color Color { get; set; }

    public bool Center { get; set; }
    
    IStandardMaterial Material { get; set; } = Injector.Get<IStandardMaterial>();

    public override void Awake()
    {
        CreateRenderObject
        (
            new RectangleData
            { 
                Size = Size, Color = Color, ModelViewProjection = ModelViewProjection, Material = Material
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
        UpdateRenderObject
         (
            new RectangleData
            { 
                Size = Size, Color = Color, ModelViewProjection = ModelViewProjection, Material = Material
            }
         );
        base.LateUpdate();
    }

    private void CenterRect()
    {
        if (Center)
            Transform.Origin = Size * Transform.Scale / 2f;
    }
}
