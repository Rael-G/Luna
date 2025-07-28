using System.Numerics;

namespace Luna;

public class PostProcessorData
{
    public IMaterial Material { get; set; }
    public ShaderSource[] Shaders { get; set; } = [];
    public Vector2 Resolution { get; set; }
    public int Samples { get; set; }
}
