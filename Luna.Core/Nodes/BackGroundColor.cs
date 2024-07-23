namespace Luna;

public class BackGroundColor : Node
{
    public Color Color { get; set; } = Colors.DimGray;

    public override void Awake()
    {
        CreateRenderObject(Color);
        base.Awake();
    }

    public override void LateUpdate()
    {
        UpdateRenderObject(Color);
        base.LateUpdate();
    }
}
