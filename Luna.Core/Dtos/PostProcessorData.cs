using System.Numerics;

namespace Luna;

public class PostProcessorData : FrameBufferData
{
    public ShaderSource[] Shaders { get; set; } = [];
    public Vector2 Resolution { get; set; }
}
