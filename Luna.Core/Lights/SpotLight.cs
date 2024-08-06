using Luna.Maths;

namespace Luna;

public class SpotLight : PointLight
{
    public float CutOff { get; set; } = (float)Math.Cos(12.5f.ToRadians());
    public float OuterCutOff { get; set; } = (float)Math.Cos(15f.ToRadians());

    public override T CreateLight<T>()
        => (new SpotLight() as T)!;
}
