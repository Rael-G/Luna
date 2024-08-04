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
        Textures.GetValueOrDefault(key)?.Dispose();
        Textures[key] = texture;
    }

    public void Bind()
    {
        Shader.Use();

        int textureUnit = 0;

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

    public override void Dispose(bool disposing)
    {
        Shader.Dispose();
        foreach (var texture in Textures.Values)
        {
            texture.Dispose();
        }
    }
}
