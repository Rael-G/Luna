using Box2DSharp.Collision.Shapes;

namespace Luna.Box2D;

public interface IShape2D
{
    public abstract Shape CreateShape();
}
