using System.Numerics;
using Luna.Maths;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

[Serialize]
public class Material() : Disposable, IMaterial
{
    public Dictionary<string, float> FloatProperties { get; private set; } = [];
    public Dictionary<string, Vector3> Vector3Properties { get; private set; } = [];
    public Dictionary<string, Color> ColorProperties { get; private set; } = [];
    public Dictionary<string, Matrix4x4> MatricesProperties { get; private set; } = [];
    public Dictionary<string, int> IntProperties { get; private set; } = [];
    public Dictionary<string, bool> BoolProperties { get; private set; } = [];

    private Dictionary<string, Texture2D> Textures2D { get; set; } = [];
    private Dictionary<string, CubeMap> CubeMaps { get; set; } = [];

    // Shoud be set only once on creation
    public ShaderSource[] Shaders 
    {
        set
        {
            Shader = new(value);
        }
    }

    private ProgramShader Shader { get; set; }

    public void SetTexture2D(string key, Texture2D texture)
    {
        if (Textures2D.TryGetValue(key, out var oldTexture) && texture.Hash != oldTexture.Hash)
        {
            TextureManager.Get(oldTexture.Hash)?.Dispose();
        }

        if (texture.Hash != oldTexture.Hash)
        {
            TextureManager.Load(texture);
        }

        Textures2D[key] = texture;
    }

    public void SetCubeMap(string key, CubeMap texture)
    {
        if (CubeMaps.TryGetValue(key, out var oldTexture) && texture.Hash != oldTexture.Hash)
        {
            TextureManager.Get(oldTexture.Hash)?.Dispose();
        }

        if (texture.Hash != oldTexture.Hash)
        {
            TextureManager.Load(texture);
        }

        CubeMaps[key] = texture;
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

        foreach (var pair in Textures2D)
        {
            var texture = TextureManager.Get(pair.Value.Hash);
            texture?.Bind(TextureUnit.Texture0 + textureUnit);
            Shader.Set(pair.Key, textureUnit);
            textureUnit++;
        }

        foreach (var pair in CubeMaps)
        {
            var texture = TextureManager.Get(pair.Value.Hash);
            texture?.Bind(TextureUnit.Texture0 + textureUnit);
            Shader.Set(pair.Key, textureUnit);
            textureUnit++;
        }

        foreach (var property in FloatProperties)
        {
            Shader.Set(property.Key, property.Value);
        }

        foreach (var property in Vector3Properties)
        {
            Shader.SetVec3(property.Key, property.Value.ToFloatArray());
        }

        foreach (var property in ColorProperties)
        {
            Shader.SetVec4(property.Key, property.Value.ToFloatArray());
        }

        foreach (var property in MatricesProperties)
        {
            Shader.SetMat4(property.Key, property.Value.ToFloatArray());
        }
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        Shader.Dispose();

        foreach (var texture in Textures2D.Values)
        {
            TextureManager.Dispose(texture.Hash);
        }

        foreach (var texture in CubeMaps.Values)
        {
            TextureManager.Dispose(texture.Hash);
        }

        base.Dispose(disposing);
    }
}
