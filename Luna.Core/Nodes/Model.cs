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

    public TextureFilter TextureFilter { get; set; } = TextureFilter.Bilinear;

    public IStandardMaterial Material { get; set; } = Injector.Get<IStandardMaterial>();

    public bool Center { get; set; }

    private string _path = string.Empty;

    public override void Awake()
    {
        Material.ModelViewProjection = ModelViewProjection;
        CreateRenderObject
        (
            new ModelData
            { 
                Path = Path, TextureFilter = TextureFilter, Material = Material
            }
        );

        base.Awake();
    }

    public override void LateUpdate()
    {
        Material.ModelViewProjection = ModelViewProjection;
        UpdateRenderObject
         (
            new ModelData
            { 
                Path = Path, TextureFilter = TextureFilter, Material = Material
            }
         );
        base.LateUpdate();
    }
}
