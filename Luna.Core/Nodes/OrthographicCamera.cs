using System.Numerics;
using System.Runtime.Serialization;

namespace Luna;

public class OrtographicCamera : Node, ICamera
{
    public float Left { get; set; } = 0.0f;
    public float Right { get; set; } = 1.0f;
    public float Bottom { get; set; } = 1.0f;
    public float Top { get; set; } = 0.0f;
    public float Near { get; set; } = -10.0f;
    public float Far { get; set; } = 10.0f;

    public Listener? Listener { get; set; }

    [IgnoreDataMember]
    public virtual Matrix4x4 Projection 
        => Matrix4x4.CreateOrthographicOffCenter(Left, Right, Bottom, Top, Near, Far);
    
    [IgnoreDataMember]
    public virtual Matrix4x4 View 
        => Matrix4x4.CreateTranslation(-Transform.GlobalPosition);

    public override void LateUpdate()
    {
        Listener?.UpdateListener(Transform);
        base.LateUpdate();
    }

}