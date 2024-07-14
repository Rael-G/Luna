using System.Reflection;

namespace Luna.Engine.OpenGl;

internal class Program
{
    public string Name { get; set; }
    public Shader[] Shaders { get; set; }

    public Program(string name, Shader[] shaders)
    {
        Name = name;
        Shaders = shaders;
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

    public static string DefaultShaderPath(string name)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)!;
        return Path.Combine(assemblyLocation, "Assets", "shaders", name);
    }

    ~Program()
    {
        ShaderManager.StopUsing(this);
    }
}