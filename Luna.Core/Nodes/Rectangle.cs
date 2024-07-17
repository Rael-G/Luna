namespace Luna;

public class Rectangle : Node2D
{
    public float Width { get; set; }
    public float Height { get; set; }
    public Color Color { get; set; }

    public override void Awake()
    {
         CreateRenderObject
         (
            new RectangleData
            { 
                Width = Width, Height = Height, Transform = TransformMatrix, Color = Color 
            }
         );

        base.Awake();
    }

    public override void Update()
    {
        Transform.Rotation += 0.001f;
    }

    public override void LateUpdate()
    {
        UpdateRenderObject
         (
            new RectangleData
            { 
                Width = Width, Height = Height, Transform = TransformMatrix, Color = Color 
            }
         );
        base.LateUpdate();
    }
}
