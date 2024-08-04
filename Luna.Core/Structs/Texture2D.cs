namespace Luna;

public struct Texture2D
{
    public Texture2D()
    {
    }

    public string Path { get; set; } = string.Empty;
    public TextureFilter TextureFilter { get; set; } = TextureFilter.Bilinear;
    public TextureWrap TextureWrap { get; set; } = TextureWrap.Repeat;
    public int MipmapLevel { get; set; }
    public bool FlipV { get; set; }

    public override readonly int GetHashCode()
    {
        return (Path + TextureFilter + TextureWrap + MipmapLevel).GetHashCode();
    }
}
