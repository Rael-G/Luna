using System.Reflection;

namespace Luna.OpenGL;

internal class ProgramShader : Disposable
{
    public string Name { get; }
    public ShaderSource[] Shaders { get; set; }

    public ProgramShader(ShaderSource[] shaders)
    {
        string name = string.Empty;
        foreach(var shader in shaders)
            name += Path.GetFileNameWithoutExtension(shader.Name);
        
        Name = name + ".bin";
        Shaders = shaders;
        ShaderManager.GetShader(this);
        ShaderManager.StartUsing(this);
    }

    public void Use()
    {
        ShaderManager.UseProgram(this);
    }

    public void SetMat4(string name, float[] mat4)
        => ShaderManager.SetMat4(this, name, mat4);
    
    public void SetVec4(string name, float[] vec4)
        => ShaderManager.SetVec4(this, name, vec4);

    public void SetVec3(string name, float[] vec3)
        => ShaderManager.SetVec3(this, name, vec3);

    public void Set(string name, int value)
        => ShaderManager.Set(this, name, value);

    public void Set(string name, bool value)
        => ShaderManager.Set(this, name, value);
    
    public void Set(string name, float value)
        => ShaderManager.Set(this, name, value);

    public static string DefaultShaderPath(string name)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)!;
        return Path.Combine(assemblyLocation, "Assets", "shaders", name);
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        ShaderManager.StopUsing(this);

        base.Dispose(disposing);
    }
}