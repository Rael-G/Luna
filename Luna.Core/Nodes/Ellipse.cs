using System.Numerics;

namespace Luna;

public class Ellipse : Node2D
{
    public Vector2 Radius { get; set; } = Vector2.One;

    public int Segments { get; set; } = 10;

    public Color Color { get; set; } = Colors.White;

    public IStandardMaterial Material { get; set; } = Injector.Get<IStandardMaterial>();

    public override void Awake()
    {
        Material.Color = Color;
        Material.ModelViewProjection = ModelViewProjection;
        CreateRenderObject
        (
            new EllipseData
            { 
                Radius = Radius, Segments = Segments, Material = Material
            }
        );

        base.Awake();
    }

    public override void LateUpdate()
    {
        Material.Color = Color;
        Material.ModelViewProjection = ModelViewProjection;
        UpdateRenderObject
         (
            new EllipseData
            { 
                Radius = Radius, Segments = Segments, Material = Material
            }
         );
        base.LateUpdate();
    }
}
