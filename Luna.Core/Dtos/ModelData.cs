namespace Luna;

public struct ModelData
{
    public string Path { get; set; }
    public FilterMode TextureFilter { get; set; }
    public IStandardMaterial Material { get; set; }
}
