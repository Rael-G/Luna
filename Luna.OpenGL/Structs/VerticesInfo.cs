namespace Luna.OpenGL;

public struct VerticesInfo
{
    public int Size;
    public int[] Lengths;
    public readonly uint Stride => (uint)Lengths.Sum();
}
