using System.Reflection;
using Silk.NET.OpenGL;

namespace Luna.OpenGl;

internal static class ShaderManager
{
    private static readonly Dictionary<string, uint> Shaders = [];
    private static readonly Dictionary<string, int> Counters = [];

    private static readonly GL _gl = Window.Gl?? throw new WindowException("Window.Gl is null.");

    private static string BinaryPath
    {
        get 
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            return Path.Combine(assemblyLocation, "bin", "shaders");
        }
    }

    public static void UseProgram(Program program)
        => _gl.UseProgram(GetShader(program));
    

    public static void StartUsing(Program program)
    {
        if (!Counters.TryGetValue(program.Name, out var count))
            Counters.Add(program.Name, 1);

        Counters[program.Name]++;
    }

    public static void StopUsing(Program program)
    {
        if (!Counters.TryGetValue(program.Name, out var count))
            return;
        
        count = --Counters[program.Name];

        if (count <= 0)
        {
            _gl.DeleteProgram(GetShader(program));
            Counters.Remove(program.Name);
            Shaders.Remove(program.Name);
        }
    }

    private static uint GetShader(Program program)
    {
        if (Shaders.TryGetValue(program.Name, out var id))
            return id;
        

        return GetFromBinary(program);
    }

    private static uint GetFromBinary(Program program)
    {
        var filePath = Path.Combine(BinaryPath, program.Name);
        if (!File.Exists(filePath))
            return GetFromSource(program);

        var fileStream = File.OpenRead(filePath);
        using var binaryReader = new BinaryReader(fileStream);

        var bynaryFormat = (GLEnum)binaryReader.ReadInt32();
        byte[] binary = binaryReader.ReadBytes((int)fileStream.Length - sizeof(int));

        uint id = _gl.CreateProgram();
        _gl.ProgramBinary<byte>(id, bynaryFormat, binary);

        if (!GlErrorUtils.CheckProgramLink(id))
        {
            _gl.DeleteProgram(id);
            GetFromSource(program);
        }

        Shaders.Add(program.Name, id);
        return id;
    }

    private static uint GetFromSource(Program program)
    {
        var programId = _gl.CreateProgram();
        var shaderIds = new List<uint>();

        foreach (var shader in program.Shaders)
        {
            if (!File.Exists(shader.Path))
            {
                Console.WriteLine($"Failed to open shader source: {shader.Path}");
                continue;
            }
            
            var source = File.ReadAllText(shader.Path);
            var shaderId =_gl.CreateShader(shader.ShaderType);
            _gl.ShaderSource(shaderId, source);
            _gl.CompileShader(shaderId);

            if (!GlErrorUtils.CheckShaderCompile(shaderId))
            {
                _gl.DeleteShader(shaderId);
                continue;
            }

            _gl.AttachShader(programId, shaderId);
            shaderIds.Add(shaderId);
        }

        _gl.LinkProgram(programId);

        foreach (var shaderId in shaderIds)
        {
            _gl.DetachShader(programId, shaderId);
            _gl.DeleteShader(shaderId);
        }
        
        if (!GlErrorUtils.CheckProgramLink(programId))
        {
            _gl.DeleteProgram(programId);
            return 0;
        }

        SaveBinary(program, programId);
        Shaders.Add(program.Name, programId);

        return programId;
    }

    private static void SaveBinary(Program program, uint programId)
    {
        _gl.GetProgram(programId, ProgramPropertyARB.ProgramBinaryLength, out var buffSize);
        byte[] binary = new byte[buffSize];
        _gl.GetProgramBinary<byte>(programId, out _, out var binaryFormat, binary);
        
        if (!Directory.Exists(BinaryPath)) Directory.CreateDirectory(BinaryPath);

        using var fileStream = new FileStream(Path.Combine(BinaryPath, program.Name), FileMode.OpenOrCreate, FileAccess.Write);
        using var binaryWriter = new BinaryWriter(fileStream);

        binaryWriter.Write((int)binaryFormat);
        binaryWriter.Write(binary);
    }

    public static void UniformMat4(Program program, string name, float[] mat4)
    {
        var programId = GetShader(program);
        var loc = _gl.GetUniformLocation(programId, name);
        _gl.UniformMatrix4(loc, false, mat4);
    }

    public static void UniformVec4(Program program, string name, float[] vec4)
    {
        var programId = GetShader(program);
        var loc = _gl.GetUniformLocation(programId, name);
        _gl.Uniform4(loc, vec4);
    }

    public static void UniformVec3(Program program, string name, float[] vec3)
    {
        var programId = GetShader(program);
        var loc = _gl.GetUniformLocation(programId, name);
        _gl.Uniform3(loc, vec3);
    }
}
