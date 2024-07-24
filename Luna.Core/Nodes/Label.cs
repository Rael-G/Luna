using System.Numerics;
using Luna.Core;

namespace Luna;

public class Label : Node2D
{
    public string Text { get; set; } = string.Empty;
    public Vector2 Size { get; set; } = new Vector2(48f, 48f);
    public Color Color { get; set; } = Colors.White;
    public bool FlipV { get; set; } = false;
    public bool CenterH { get; set; }
    public bool CenterV { get; set; }

    public string Path
    {
        get => _path;
        set
        {
            _path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), value);
        }
    }

    private string _path = string.Empty;

    public Label(string path)
    {
        Path = path;
    }

    public override void Awake()
    {
        CreateRenderObject
        (
            new TextData
            {
                Text = Text, Path = Path, Color = Color, Transform = TransformMatrix, 
                Size = Size, FlipV = FlipV
            }
        );

        base.Awake();
    }

    public override void EarlyUpdate()
    {
        CenterText();
        base.EarlyUpdate();
    }

    public override void LateUpdate()
    {
        UpdateRenderObject
        (
            new TextData
            {
                Text = Text, Path = Path, Color = Color, Transform = TransformMatrix, 
                Size = Size, FlipV = FlipV
            }
        );

        base.LateUpdate();
    }

    private void CenterText()
    {
        if (!CenterH && !CenterV)
            return;
            
        var utils = Injector.Get<IEngineUtils>();
        Vector2 origin = Vector2.Zero;
        if (CenterH)
            origin += new Vector2(utils.MeasureTextSize((Path, Size), Text).X, 0f) * Transform.Scale / 2;
        if (CenterV)
            origin += new Vector2(0f, utils.MeasureTextSize((Path, Size), Text).Y) * Transform.Scale / 2;

        Transform.Origin = origin;
    }

}
