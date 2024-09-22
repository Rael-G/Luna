namespace Luna;

public struct CubeMap
{
    public CubeMap()
    {
        
    }

    public string[] Paths { get; set; } = [];
    public TextureFilter TextureFilter { get; set; } = TextureFilter.Bilinear;
    public TextureWrap TextureWrap { get; set; } = TextureWrap.Clamp;
    public int MipmapLevel { get; set; }
    public bool FlipV { get; set; }
    public ImageType ImageType { get; set; } = ImageType.SRGB;

    public string Hash
    { 
        readonly get => string.IsNullOrEmpty(_hash)? GetHashCode().ToString() : _hash;
        set => _hash = value;
    }

    private string? _hash;

    public override readonly int GetHashCode()
    {
        return (string.Join("", Paths) + TextureFilter + TextureWrap + MipmapLevel + FlipV).GetHashCode();
    }
}
