using System.Numerics;
using Box2DSharp.Collision.Shapes;

namespace Luna.Box2D;

public class PolygonShape2D(Vector2[] vertices) : IShape2D
{
    private Vector2[] _vertices { get; set; } = vertices;

    public Shape CreateShape()
    {
        var polygonShape = new PolygonShape();
        var verticesInMeters = _vertices.Select(v => v.ToMeters()).ToArray();
        polygonShape.Set(verticesInMeters);
        return polygonShape;
    }
}
