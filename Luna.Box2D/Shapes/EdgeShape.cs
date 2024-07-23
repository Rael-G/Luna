using System.Numerics;
using Box2DSharp.Collision.Shapes;

namespace Luna.Box2D;

public class EdgeShape2D : IShape2D
{
    private readonly Vector2 _vertex0;
    private readonly Vector2 _vertex1;
    private readonly Vector2 _vertex2;
    private readonly Vector2 _vertex3;

    private readonly bool _oneSided = false;
    
    // Create two sided edge shape
    public EdgeShape2D(Vector2 vertex0, Vector2 vertex1)
    {
        _vertex0 = vertex0;
        _vertex2 = vertex1;
    }

    // Create one sided edge shape
    public EdgeShape2D(Vector2 vertex0, Vector2 vertex1, Vector2 vertex2, Vector2 vertex3)
    {
        _vertex0 = vertex0;
        _vertex1 = vertex1;
        _vertex2 = vertex2;
        _vertex3 = vertex3;
        _oneSided = true;
    }

    public Shape CreateShape()
    {
        var edgeShape = new EdgeShape();

        if (_oneSided)
            edgeShape.SetOneSided(_vertex0.ToMeters(), _vertex1.ToMeters(), _vertex2.ToMeters(), _vertex3.ToMeters());
        
        else
            edgeShape.SetTwoSided(_vertex0.ToMeters(), _vertex1.ToMeters());

        return edgeShape;
    }
}
