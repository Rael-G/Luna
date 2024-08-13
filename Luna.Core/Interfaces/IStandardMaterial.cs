namespace Luna;

public interface IStandardMaterial : IMaterial
{
    public ModelViewProjection ModelViewProjection { get; set; }
    public Texture2D[] DiffuseMaps { get; set; }
    public Texture2D[] SpecularMaps { get; set; }
    public Color Color { get; set; }
    public float Shininess { get; set; }
    public bool IsAffectedByLight { get; set; }
}
