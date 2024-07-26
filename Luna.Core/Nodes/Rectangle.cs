using System.Numerics;

namespace Luna;

public class Rectangle : Node2D
{
    public Vector2 Size { get; set; }

    public Color Color { get; set; }

    public bool Center { get; set; }

    public override void Awake()
    {
        CreateRenderObject
        (
            new RectangleData
            { 
                Size = Size, Transform = TransformMatrix, Color = Color
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
                Size = Size, Transform = TransformMatrix, Color = Color
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
