namespace Luna;

public class BackGroundColor : Node
{
    public Color Color { get; set; } = Colors.Gray;

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
