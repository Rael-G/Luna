namespace Luna;

public class PointLight : DirectionalLight
{
    public float Constant { get; set; } = 1f;
    public float Linear { get; set; } = 0.09f;
    public float Quadratic { get; set; } = 0.032f;
}
