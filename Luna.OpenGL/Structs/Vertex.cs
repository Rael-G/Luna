using System.Numerics;

namespace Luna.OpenGL;

public struct Vertex
{
    public Vector3 Position { get; set; }
    public Vector3 Normal { get; set; }
    public Vector2 TexCoords { get; set; }
    public Vector3 Tangent;
    public Vector3 Bitangent;
}
