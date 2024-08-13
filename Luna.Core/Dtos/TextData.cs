using System.Numerics;
using Luna.Maths;

namespace Luna;

using FontKey = (string Name, Vector2 Size);

public struct TextData
{
    public string Path { get; set; }
    public Vector2 Size { get; set; }
    public Color Color { get; set; }
    public string Text { get; set; }
    public Matrix Transform { get; set; }
    public bool FlipV { get; set; }
    public readonly FontKey FontKey => (Path, Size);
}
