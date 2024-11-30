using System.Numerics;

namespace Luna;

public struct CubeMap
{
    public CubeMap()
    {
        
    }

    public string[] Paths { get; set; } = [];
    public Vector2 Size { get; set; }
    public FilterMode FilterMode { get; set; } = FilterMode.Bilinear;
    public WrapMode WrapMode { get; set; } = WrapMode.ClampToEdge;
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
        var str = string.Empty;
        if (Paths is not null)
        {
            str = string.Join("", Paths);
        }
        str += FilterMode;
        str += WrapMode;
        str += FlipV;

        return str.GetHashCode();
    }
}
