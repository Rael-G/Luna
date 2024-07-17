using Luna.Maths;

namespace Luna;

public struct TextData
{
    public readonly string Name => System.IO.Path.GetFileNameWithoutExtension(Path);
    public string Path { get; set; }
    public int PixelSize { get; set; }
    public Color Color { get; set; }
    public string Text { get; set; }
    public Matrix Transform { get; set; }
    public bool FlipV { get; set; }
}
