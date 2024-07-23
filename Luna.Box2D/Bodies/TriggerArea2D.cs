namespace Luna.Box2D;

public class TriggerArea2D : StaticBody2D
{
    public TriggerArea2D()
    {
        FixtureDef.IsSensor = true;
    }
}
