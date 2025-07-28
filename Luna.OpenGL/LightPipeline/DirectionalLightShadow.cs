using System.Numerics;

namespace Luna.OpenGL;

public class DirectionalLightShadow(DirectionalLight light)
{
    public DirectionalLight Light { get; set; } = light;
    public Matrix4x4 LightSpaceMatrix { get; set; }
    public Texture2D ShadowMap { get; set; }
}
