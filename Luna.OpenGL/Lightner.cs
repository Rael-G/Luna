using System.Numerics;

namespace Luna.OpenGL;

public class DirectionalLightShadow(DirectionalLight light)
{
    public DirectionalLight Light { get; set; } = light;
    public Matrix4x4 LightSpaceMatrix { get; set; }
    public Texture2D ShadowMap { get; set; }
}

public class PointLightShadow(PointLight light)
{
    public PointLight Light { get; set; } = light;
    public float FarPlane { get; set; }
    public CubeMap ShadowMap { get; set; }
}

public class SpotLightShadow(SpotLight light)
{
    public SpotLight Light { get; set; } = light;
}

public class LightEmitter
{
    public static DirectionalLightShadow? DirLight { get; set; }
    public static readonly List<PointLightShadow> PointLights = [];
    public static readonly List<SpotLightShadow> SpotLights = [];

    //Temporary limit for the actual improvised light system
    private const int DirectionalMaxLenght = 1;
    private const int PointMaxLenght = 10;
    private const int SpotMaxLenght = 10;
}
