using System.Numerics;
using Box2DSharp.Collision.Shapes;

namespace Luna.Box2D;

public class BoxShape2D(Vector2 size) : IShape2D
{
    public Shape CreateShape()
    {
        var boxShape = new PolygonShape();
        var sizeInMeters = size.ToMeters();
        boxShape.SetAsBox(sizeInMeters.X / 2, sizeInMeters.Y / 2);
        return boxShape;
    }
}
    
