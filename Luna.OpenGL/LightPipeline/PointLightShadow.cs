namespace Luna.OpenGL;

public class PointLightShadow(PointLight light)
{
    public PointLight Light { get; set; } = light;
    public float FarPlane { get; set; }
    public CubeMap ShadowMap { get; set; }
}
