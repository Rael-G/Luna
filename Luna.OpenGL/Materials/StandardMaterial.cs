namespace Luna.OpenGL;

public class StandardMaterial : Material, IStandardMaterial
{
    private static readonly string DefaultTexturePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/images/DefaultTexture.png");

    public Texture2D[] DiffuseMaps 
    {
        get => _diffuse;
        set
        {
            _diffuse = value;
            for(int i = 0; i < _diffuse.Length; i++)
                Set("material.diffuse" + i, Texture.Load(_diffuse[i]));
        } 
    }

    public Texture2D[] SpecularMaps 
    { 
        get => _specullar;
        set 
        {
            _specullar = value;
            for(int i = 0; i < _specullar.Length; i++)
                Set("material.specular" + i , Texture.Load(_specullar[i]));
        }
    }
    
    public Color Color 
    {
        get => _color;
        set => ColorProperties["material.color"] = value; 
    }

    public float Shininess 
    {
        get => _shininess;
        set 
        {
            _shininess = value;
            FloatProperties["material.shininess"] = _shininess;
        }
    }

    public bool IsAffectedByLight 
    { 
        get => _isAffectedByLight;
        set
        {
            _isAffectedByLight = value;
            BoolProperties["isAffectedByLight"] = _isAffectedByLight;
        }
    }

    private Texture2D[] _diffuse = [ new(){ Path = DefaultTexturePath } ];
    private Texture2D[] _specullar = [ new(){ Path = DefaultTexturePath } ];
    private Color _color = Colors.White;
    private float _shininess = 32;
    private bool _isAffectedByLight = true;

    public StandardMaterial() 
        : base(Injector.Get<ShaderSource[]>())
    {
        DiffuseMaps = _diffuse;
        SpecularMaps = _specullar;
        Color = _color;
        Shininess = _shininess;
        IsAffectedByLight = _isAffectedByLight;
    }
}
