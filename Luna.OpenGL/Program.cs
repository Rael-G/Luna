using System.Reflection;
using Luna.Core;

namespace Luna.OpenGL;

internal class Program : Disposable
{
    public string Name { get; set; }
    public Shader[] Shaders { get; set; }

    public Program(Shader[] shaders)
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

    public void UniformMat4(string name, float[] value)
        => ShaderManager.UniformMat4(this, name, value);
    
    public void UniformVec4(string name, float[] value)
        => ShaderManager.UniformVec4(this, name, value);

    public void UniformVec3(string name, float[] vec3)
        => ShaderManager.UniformVec3(this, name, vec3);

    public static string DefaultShaderPath(string name)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)!;
        return Path.Combine(assemblyLocation, "Assets", "shaders", name);
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        ShaderManager.StopUsing(this);
    }
}