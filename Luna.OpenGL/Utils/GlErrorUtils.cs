using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal static class GlErrorUtils
{
    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    public static bool CheckError(string issue = "")
    {
        GLEnum error = _gl.GetError();
        if (error != GLEnum.NoError)
            Console.WriteLine($"OpenGL Error: {error}. " + (!string.IsNullOrEmpty(issue)? $" At {issue}.": ""));
        return error != GLEnum.NoError;
    }

    public static void Check(uint handle, GLEnum type)
    {
        if (!CheckStatus(type, handle))
            Console.WriteLine($"A binded {type} is diferent from the expected.");
    }

    public static bool CheckProgramLink(uint programId)
    {
        _gl.GetProgram(programId, ProgramPropertyARB.LinkStatus, out int success);

        if (success is 0)
        {
            string infoLog = _gl.GetProgramInfoLog(programId);
            Console.WriteLine($"Program link error: {infoLog}");
            return false;
        }
        
        return true;
    }

    public static bool CheckShaderCompile(uint shaderId)
    {
        _gl.GetShader(shaderId, ShaderParameterName.CompileStatus, out int success);

            if (success is 0)
            {
                string infoLog = _gl.GetShaderInfoLog(shaderId);
                Console.WriteLine($"Shader compile error: {infoLog}");
                return false;
            }

            return true;
    }

    private static bool CheckStatus(GLEnum pname, uint expected) 
    {
        _gl.GetInteger(pname, out int data);
        return data == expected;
    }
}
