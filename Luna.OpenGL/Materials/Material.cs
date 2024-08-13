using System.Numerics;
using Luna.Maths;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

public class Material(ShaderSource[] shaders) : Disposable, IMaterial
{
    public Dictionary<string, float> FloatProperties { get; private set; } = [];
    public Dictionary<string, Vector3> Vector3Properties { get; private set; } = [];
    public Dictionary<string, Color> ColorProperties { get; private set; } = [];
    public Dictionary<string, Matrix> MatricesProperties { get; private set; } = [];
    public Dictionary<string, int> IntProperties { get; private set; } = [];
    public Dictionary<string, bool> BoolProperties { get; private set; } = [];

    private Dictionary<string, Texture> Textures { get; set; } = [];

    private ProgramShader Shader { get; set; } = new(shaders);

    public void Set(string key, Texture texture)
    {
        var t = Textures.GetValueOrDefault(key);
        if (texture.GetHashCode() != t?.GetHashCode())
            t?.Dispose();
        Textures[key] = texture;
    }

    public virtual void Bind()
    {
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
