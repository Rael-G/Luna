using System.Numerics;
using System.Runtime.InteropServices;

namespace Luna.OpenGL;

public struct Vertex
{
    public Vector3 Position { get; set; }
    public Vector3 Normal { get; set; }
    public Vector2 TexCoords { get; set; }

    public const int Params = 3;
    public static readonly int[] Lengths = [3, 3, 2];
    public static uint Stride => (uint)Marshal.SizeOf(typeof(Vertex));
}
