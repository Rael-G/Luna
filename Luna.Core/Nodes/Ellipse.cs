using System.Numerics;

namespace Luna;

public class Ellipse : Node2D
{
    public Vector2 Radius { get; set; } = Vector2.One;

    public int Segments { get; set; } = 10;

    public Color Color { get; set; }

    IStandardMaterial Material { get; set; } = Injector.Get<IStandardMaterial>();

    public override void Awake()
    {
        CreateRenderObject
        (
            new EllipseData
            { 
                Radius = Radius, Segments = Segments, ModelViewProjection = ModelViewProjection, Color = Color, Material = Material
            }
        );

        base.Awake();
    }

    public override void LateUpdate()
    {
        UpdateRenderObject
         (
            new EllipseData
            { 
                Radius = Radius, Segments = Segments, ModelViewProjection = ModelViewProjection, Color = Color, Material = Material
            }
         );
        base.LateUpdate();
    }
}
