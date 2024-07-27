using System.Numerics;

namespace Luna.Core;

public class Ellipse : Node2D
{
    public Vector2 Radius { get; set; } = Vector2.One;

    public int Segments { get; set; } = 10;

    public Color Color { get; set; }

    public override void Awake()
    {
        CreateRenderObject
        (
            new EllipseData
            { 
                Radius = Radius, Segments = Segments, Transform = TransformMatrix, Color = Color 
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
                Radius = Radius, Segments = Segments, Transform = TransformMatrix, Color = Color 
            }
         );
        base.LateUpdate();
    }
}
