using System.Runtime.Serialization;

namespace Luna;

public class PointLight : DirectionalLight
{
    public float Constant { get; set; } = 1f;
    public float Linear { get; set; } = 0.09f;
    public float Quadratic { get; set; } = 0.032f;

    [IgnoreDataMember]
    public float Radius 
    {
        get
        {
            float intensityThreshold = 0.01f;
            return (-Linear + MathF.Sqrt(Linear * Linear - 4 * Quadratic * (Constant - 1 / intensityThreshold)))
                   / (2 * Quadratic);
        }
    }
}
