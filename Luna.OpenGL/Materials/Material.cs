using System.Numerics;
using Luna.Maths;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class Material : Disposable, IMaterial
{
    public Dictionary<string, float> FloatProperties { get; private set; } = [];
    public Dictionary<string, Vector3> Vector3Properties { get; private set; } = [];
    public Dictionary<string, Color> ColorProperties { get; private set; } = [];
    public Dictionary<string, Matrix> MatricesProperties { get; private set; } = [];
    public Dictionary<string, int> IntProperties { get; private set; } = [];
    public Dictionary<string, bool> BoolProperties { get; private set; } = [];

    private Dictionary<string, Texture> Textures { get; set; } = [];

    private ProgramShader Shader { get; set; }

    public ModelViewProjection ModelViewProjection 
    {
        get => _modelViewProjection;
        set
        {
            _modelViewProjection = value;
            MatricesProperties["model"] = _modelViewProjection.Model.Transpose();
            MatricesProperties["view"] = _modelViewProjection.View.Transpose();
            MatricesProperties["projection"] = _modelViewProjection.Projection.Transpose();
            Vector3Properties["viewPos"] = _modelViewProjection.CameraPosition;
        }
    }

    private ModelViewProjection _modelViewProjection = new() 
    { 
        Model = Matrix.Identity(4), 
        View = Matrix.Identity(4), 
        Projection = Matrix.Identity(4),
        CameraPosition = Vector3.Zero
    };

    public Material(ShaderSource[] shaders)
    {
        Shader = new(shaders);
        ModelViewProjection = _modelViewProjection;
    }

    public void Set(string key, Texture texture)
    {
        var t = Textures.GetValueOrDefault(key);
        if (texture.GetHashCode() != t?.GetHashCode())
            t?.Dispose();
        Textures[key] = texture;
    }

    public void Bind()
    {
        Light();
        
        Shader.Use();

        int textureUnit = 0;

        foreach (var property in IntProperties)
        {
            Shader.Set(property.Key, property.Value);
        }

        foreach (var property in BoolProperties)
        {
            Shader.Set(property.Key, property.Value);
        }

        foreach (var texture in Textures)
        {
            texture.Value.Bind(TextureUnit.Texture0 + textureUnit);
            Shader.Set(texture.Key, textureUnit);
            textureUnit++;
        }

        foreach (var property in FloatProperties)
        {
            Shader.Set(property.Key, property.Value);
        }

        foreach (var property in Vector3Properties)
        {
            Shader.SetVec3(property.Key, property.Value.ToMatrix());
        }

        foreach (var property in ColorProperties)
        {
            Shader.SetVec4(property.Key, property.Value.ToMatrix());
        }

        foreach (var property in MatricesProperties)
        {
            Shader.SetMat4(property.Key, property.Value);
        }
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

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;
        Shader.Dispose();
        foreach (var texture in Textures.Values)
        {
            texture.Dispose();
        }

        base.Dispose(disposing);
    }
}
