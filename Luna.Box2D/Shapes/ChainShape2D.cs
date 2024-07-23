using System.Numerics;
using Box2DSharp.Collision.Shapes;

namespace Luna.Box2D;

public class ChainShape2D(Vector2[] vertices, Vector2 previousVertex, Vector2 nextVertex) : IShape2D
{
    public Shape CreateShape()
    {
        var chainShape = new ChainShape();
        var verticesInMeters = vertices.Select(v => v.ToMeters()).ToArray();
        chainShape.CreateChain(verticesInMeters, verticesInMeters.Length, previousVertex, nextVertex);
        return chainShape;
    }
}
