namespace Luna;

public class Model : Node
{
    public string Path
    {
        get => _path;
        set
        {
            _path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), value);
        }
    }

    public FilterMode TextureFilter { get; set; } = FilterMode.Bilinear;

    public IStandardMaterial Material { get; set; } = Injector.Get<IStandardMaterial>();

    public bool Center { get; set; }

    private string _path = string.Empty;

    public override void Awake()
    {
        CreateRenderObject
        (
            new ModelData
            { 
                Path = Path, TextureFilter = TextureFilter, Material = Material, ModelViewProjection = ModelViewProjection
            }
        );

        base.Awake();
    }

    public override void LateUpdate()
    {
        UpdateRenderObject
         (
            new ModelData
            { 
                Path = Path, TextureFilter = TextureFilter, Material = Material, ModelViewProjection = ModelViewProjection
            }
         );
        base.LateUpdate();
    }
}
