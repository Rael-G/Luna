using System;

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

    public override readonly int GetHashCode()
    {
        var paths = "";
        foreach(var path in Paths)
            paths += path;

        return (paths + TextureFilter + TextureWrap + MipmapLevel + FlipV).GetHashCode();
    }
}
