namespace Luna.OpenGL;

public class StandardMaterial : Material, IStandardMaterial
{
    private const string VertexName = "StandardVertexShader.glsl";
    private const string FragmentName = "StandardFragmentShader.glsl";

    private static readonly string DefaultTexturePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/images/DefaultTexture.png");

    private static readonly ShaderSource[] Shaders = 
    [
        new()
        {
            Name = VertexName,
            Path = ProgramShader.DefaultShaderPath(VertexName),
            ShaderType = ShaderType.VertexShader
        },
        new()
        {
            Name = FragmentName,
            Path = ProgramShader.DefaultShaderPath(FragmentName),
            ShaderType = ShaderType.FragmentShader
        }
    ];

    public Texture2D Diffuse 
    {
        get => _diffuse;
        set
        {
            _diffuse = value;
            Set("material.diffuse", Texture.Load(_diffuse));
        } 
    }

    public Texture2D Specular 
    { 
        get => _specullar;
        set 
        {
            _specullar = value;
            Set("material.specular", Texture.Load(_specullar));
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

    private Texture2D _diffuse = new(){ Path = DefaultTexturePath };
    private Texture2D _specullar = new(){ Path = DefaultTexturePath };
    private Color _color = Colors.White;
    private float _shininess = 32;
    private bool _isAffectedByLight = true;

    public StandardMaterial() 
        : base(Shaders)
    {
        Diffuse = _diffuse;
        Specular = _specullar;
        Color = _color;
        Shininess = _shininess;
        IsAffectedByLight = _isAffectedByLight;
    }
}
