using System.Numerics;
using Luna.Maths;

namespace Luna.OpenGL;

public class StandardMaterial : Material, IStandardMaterial
{
    private static readonly string DefaultTexturePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/images/DefaultTexture.png");

    public Texture2D[] DiffuseMaps 
    {
        get => _diffuse;
        set
        {
            _diffuse = value;
            for(int i = 0; i < _diffuse.Length; i++)
                SetTexture2D("material.diffuse" + i, _diffuse[i]);
        } 
    }

    public Texture2D[] SpecularMaps 
    { 
        get => _specullar;
        set 
        {
            _specullar = value;
            for(int i = 0; i < _specullar.Length; i++)
                SetTexture2D("material.specular" + i , _specullar[i]);
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

    public ModelViewProjection ModelViewProjection 
    {
        get => _modelViewProjection;
        set
        {
            _modelViewProjection = value;
            MatricesProperties["model"] = _modelViewProjection.Model;
            MatricesProperties["view"] = _modelViewProjection.View;
            MatricesProperties["projection"] = _modelViewProjection.Projection;
            Vector3Properties["viewPos"] = _modelViewProjection.CameraPosition;
        }
    }

    private ModelViewProjection _modelViewProjection = new() 
    { 
        Model = Matrix4x4.Identity, 
        View = Matrix4x4.Identity, 
        Projection = Matrix4x4.Identity,
        CameraPosition = Vector3.Zero
    };

    private Texture2D[] _diffuse = [ new(){ Path = DefaultTexturePath } ];
    private Texture2D[] _specullar = [ new(){ Path = DefaultTexturePath } ];
    private Color _color = Colors.White;
    private float _shininess = 32;
    private bool _isAffectedByLight = true;

    public StandardMaterial() 
    {
        DiffuseMaps = _diffuse;
        SpecularMaps = _specullar;
        Color = _color;
        Shininess = _shininess;
        IsAffectedByLight = _isAffectedByLight;
        ModelViewProjection = _modelViewProjection;
        Shaders = Injector.Get<ShaderSource[]>();
    }

    public override void Bind()
    {
        Light();
        base.Bind();
    }

    public void Light()
    {
        DirLight();
        PointLight();
        SpotLight();
    }

    public void DirLight()
    {
        var dirLightLength = 0;
        
        foreach(var pair in LightEmitter.DirLights)
        {
            var light = pair.Value;

            Vector3Properties[$"dirLights[{dirLightLength}].direction"] = light.Direction;
            Vector3Properties[$"dirLights[{dirLightLength}].ambient"] = light.Ambient;
            Vector3Properties[$"dirLights[{dirLightLength}].diffuse"] = light.Diffuse;
            Vector3Properties[$"dirLights[{dirLightLength}].specular"] = light.Specular;
            
            dirLightLength++;
        }

        IntProperties["dirLightLength"] = dirLightLength;
    }

    public void PointLight()
    {
        var pointLightLength = 0;
        
        foreach(var pair in LightEmitter.PointLights)
        {
            var light = pair.Value;
            Vector3Properties[$"pointLights[{pointLightLength}].position"] = light.Position;
            Vector3Properties[$"pointLights[{pointLightLength}].ambient"] = light.Ambient;
            Vector3Properties[$"pointLights[{pointLightLength}].diffuse"] = light.Diffuse;
            Vector3Properties[$"pointLights[{pointLightLength}].specular"] = light.Specular;

            FloatProperties[$"pointLights[{pointLightLength}].constant"] = light.Constant;
            FloatProperties[$"pointLights[{pointLightLength}].linear"] = light.Linear;
            FloatProperties[$"pointLights[{pointLightLength}].quadratic"] = light.Quadratic;

            pointLightLength++;
        }

        IntProperties["pointLightLength"] = pointLightLength;
    }

    public void SpotLight()
    {
        var spotLightLength = 0;

        foreach(var pair in LightEmitter.SpotLights)
        {
            var light = pair.Value;

            Vector3Properties[$"spotLights[{spotLightLength}].position"] = light.Position;
            Vector3Properties[$"spotLights[{spotLightLength}].ambient"] = light.Ambient;
            Vector3Properties[$"spotLights[{spotLightLength}].diffuse"] = light.Diffuse;
            Vector3Properties[$"spotLights[{spotLightLength}].specular"] = light.Specular;

            FloatProperties[$"spotLights[{spotLightLength}].constant"] = light.Constant;
            FloatProperties[$"spotLights[{spotLightLength}].linear"] = light.Linear;
            FloatProperties[$"spotLights[{spotLightLength}].quadratic"] = light.Quadratic;

            Vector3Properties[$"spotLights[{spotLightLength}].direction"] = light.Direction;
            FloatProperties[$"spotLights[{spotLightLength}].cutOff"] = light.CutOff;
            FloatProperties[$"spotLights[{spotLightLength}].outerCutOff"] = light.OuterCutOff;

            spotLightLength++;
        }

        IntProperties["spotLightLength"] = spotLightLength;

    }
}
