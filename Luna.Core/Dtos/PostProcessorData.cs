using System.Numerics;

namespace Luna;

public class PostProcessorData
{
    public ShaderSource[] Shaders { get; set; } = [];
    public Vector2 Resolution { get; set; }
    public bool MSAA { get; set; }
}
