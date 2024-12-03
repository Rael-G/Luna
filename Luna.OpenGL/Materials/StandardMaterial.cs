using System.Numerics;

namespace Luna.OpenGL;

public class StandardMaterial : Material, IStandardMaterial
{
    private static readonly string DefaultTexturePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/images/DefaultTexture.png");

    public Texture2D[] DiffuseMaps 
    {
        get => _diffuseMaps;
        set
        {
            _diffuseMaps = value;
            for(int i = 0; i < _diffuseMaps.Length; i++)
            {
                SetTexture2D("material.diffuse" + i, _diffuseMaps[i]);
            }
        } 
    }

    public Texture2D[] SpecularMaps 
    { 
        get => _specullarMaps;
        set 
        {
            _specullarMaps = value;
            for(int i = 0; i < _specullarMaps.Length; i++)
            {
                SetTexture2D("material.specular" + i , _specullarMaps[i]);
            }
        }
    }

    public Texture2D[] NormalMaps
    {
        get => _normalMaps;
        set 
        {
            _normalMaps = value;
            if (_normalMaps.Length > 0)
            {
                for(int i = 0; i < _normalMaps.Length; i++)
                {
                    SetTexture2D("material.normalMap" + i , _normalMaps[i]);
                }
                BoolProperties["useNormalMap"] = true;
            }
            else
            {
                BoolProperties["useNormalMap"] = false;
            }
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

    private Texture2D[] _diffuseMaps = [ new(){ Path = DefaultTexturePath } ];
    private Texture2D[] _specullarMaps = [ new(){ Path = DefaultTexturePath } ];
    private Texture2D[] _normalMaps = [];
    private Color _color = Colors.White;
    private float _shininess = 32;
    private bool _isAffectedByLight = true;

    public StandardMaterial() 
    {
        DiffuseMaps = _diffuseMaps;
        SpecularMaps = _specullarMaps;
        NormalMaps = _normalMaps;
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

    private void Light()
    {
        DirLight();
        PointLight();
        SpotLight();
    }

    private void DirLight()
    {
        var dir = LightEmitter.DirLight;
        if (dir is null) return;

        Vector3Properties[$"dirLight.direction"] = dir.Light.Direction;
        Vector3Properties[$"dirLight.ambient"] = dir.Light.Ambient;
        Vector3Properties[$"dirLight.diffuse"] = dir.Light.Diffuse;
        Vector3Properties[$"dirLight.specular"] = dir.Light.Specular;

        MatricesProperties[$"dirLight.lightSpaceMatrix"] = dir.LightSpaceMatrix;
        MatricesProperties[$"lightSpaceMatrix"] = dir.LightSpaceMatrix;
        SetTexture2D($"dirLight.shadowMap", dir.ShadowMap);
    }

    private void PointLight()
    {
        var pointLightLength = 0;
        
        foreach(var point in LightEmitter.PointLights)
        {
            Vector3Properties[$"pointLights[{pointLightLength}].position"] = point.Light.Position;
            Vector3Properties[$"pointLights[{pointLightLength}].ambient"] = point.Light.Ambient;
            Vector3Properties[$"pointLights[{pointLightLength}].diffuse"] = point.Light.Diffuse;
            Vector3Properties[$"pointLights[{pointLightLength}].specular"] = point.Light.Specular;

            FloatProperties[$"pointLights[{pointLightLength}].constant"] = point.Light.Constant;
            FloatProperties[$"pointLights[{pointLightLength}].linear"] = point.Light.Linear;
            FloatProperties[$"pointLights[{pointLightLength}].quadratic"] = point.Light.Quadratic;

            FloatProperties[$"pointLights[{pointLightLength}].farPlane"] = point.FarPlane;
            SetCubeMap($"pointLights[{pointLightLength}].shadowMap", point.ShadowMap);

            pointLightLength++;
        }

        IntProperties["pointLightLength"] = pointLightLength;
    }

    private void SpotLight()
    {
        var spotLightLength = 0;

        foreach(var spot in LightEmitter.SpotLights)
        {
            Vector3Properties[$"spotLights[{spotLightLength}].position"] = spot.Light.Position;
            Vector3Properties[$"spotLights[{spotLightLength}].ambient"] = spot.Light.Ambient;
            Vector3Properties[$"spotLights[{spotLightLength}].diffuse"] = spot.Light.Diffuse;
            Vector3Properties[$"spotLights[{spotLightLength}].specular"] = spot.Light.Specular;

            FloatProperties[$"spotLights[{spotLightLength}].constant"] = spot.Light.Constant;
            FloatProperties[$"spotLights[{spotLightLength}].linear"] = spot.Light.Linear;
            FloatProperties[$"spotLights[{spotLightLength}].quadratic"] = spot.Light.Quadratic;

            Vector3Properties[$"spotLights[{spotLightLength}].direction"] = spot.Light.Direction;
            FloatProperties[$"spotLights[{spotLightLength}].cutOff"] = spot.Light.CutOff;
            FloatProperties[$"spotLights[{spotLightLength}].outerCutOff"] = spot.Light.OuterCutOff;

            spotLightLength++;
        }

        IntProperties["spotLightLength"] = spotLightLength;
    }
}
