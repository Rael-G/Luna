using Box2DSharp.Collision.Shapes;

namespace Luna.Box2D;

 public class CircleShape2D(float radius) : IShape2D
{
    public Shape CreateShape()
    {
        var circleShape = new CircleShape
        {
            Radius = radius.ToMeters()
        };
        return circleShape;
    }
}