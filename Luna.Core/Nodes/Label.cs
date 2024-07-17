using System.Numerics;

namespace Luna;

public class Label : Node2D
{
    public string Text { get; set; } = string.Empty;
    public int PixelSize { get; set; } = 48;
    public string Path { get; set; } = string.Empty;
    public Color Color { get; set; } = Colors.White;
    public Vector2 Scale { get; set; } = Vector2.One;
    public bool FlipV { get; set; } = false;

    public override void Awake()
    {
        CreateRenderObject
        (
            new TextData
            {
                Text = Text, Path = Path, Color = Color, Transform = TransformMatrix, 
                PixelSize = PixelSize, FlipV = FlipV
            }
        );

        base.Awake();
    }

    public override void LateUpdate()
    {
        UpdateRenderObject
        (
            new TextData
            {
                Text = Text, Path = Path, Color = Color, Transform = TransformMatrix, 
                PixelSize = PixelSize, FlipV = FlipV
            }
        );

        base.LateUpdate();
    }

}
