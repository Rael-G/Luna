using System.Numerics;

namespace Luna;

public class Box : Node
{
    public Vector3 Size { get; set; } = Vector3.One;

    public Color Color { get; set; } = Colors.White;

    public IStandardMaterial Material { get; set; } = Injector.Get<IStandardMaterial>();

    public bool Center { get; set; }

    public override void Awake()
    {
        Material.Color = Color;
        CreateRenderObject
        (
            new BoxData
            { 
                Size = Size, Material = Material, ModelViewProjection = ModelViewProjection
            }
        );

        base.Awake();
    }

    public override void EarlyUpdate()
    {
        CenterBox();
        base.EarlyUpdate();
    }

    public override void LateUpdate()
    {
        Material.Color = Color;
        UpdateRenderObject
         (
            new BoxData
            { 
                Size = Size, Material = Material, ModelViewProjection = ModelViewProjection
            }
         );
        base.LateUpdate();
    }

    private void CenterBox()
    {
        if (Center)
            Transform.Origin = Size * Transform.Scale / 2f * new Vector3(1f, 1f, -1f);
    }
}
