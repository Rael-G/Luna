using System.Numerics;
using Luna.Maths;

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

    public virtual Matrix4x4 Projection 
        => Matrix4x4.CreateOrthographicOffCenter(Left, Right, Bottom, Top, Near, Far);
    

    public virtual Matrix4x4 View 
        => Matrix4x4.CreateTranslation(-Transform.GlobalPosition);
    
    protected override Node? Parent 
    { 
        get => base.Parent; 
        set
        {
            base.Parent = value;
            if (Parent is not null)
                Parent.Camera = this;
        } 
    }

    public override void LateUpdate()
    {
        Listener?.UpdateListener(Transform);
        base.LateUpdate();
    }

}