namespace Luna;

public interface IStandardMaterial : IMaterial
{
    public Texture2D Diffuse { get; set; }
    public Texture2D Specular { get; set; }
    public Color Color { get; set; }
    public float Shininess { get; set; }
    public bool IsAffectedByLight { get; set; }
}
