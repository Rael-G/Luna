using System.Numerics;

namespace Luna;

public struct Texture2D
{
    public Texture2D()
    {
    }

    public string Path { get; set; } = string.Empty;
    public Vector2 Size { get; set; }
    public TextureFilter TextureFilter { get; set; } = TextureFilter.Bilinear;
    public TextureWrap TextureWrap { get; set; } = TextureWrap.Repeat;
    public Color BorderColor { get; set; } = Colors.White;
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

        return (Path + Size + TextureFilter + TextureWrap + MipmapLevel).GetHashCode();
    }
}
