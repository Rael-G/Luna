using System.Numerics;
using Luna.Maths;

namespace Luna;

public class PerspectiveCamera : Node, ICamera
{
    
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

    public Vector3 Target { get; set; } = Vector3.UnitZ;

    public Vector3 Up { get; set; } = Vector3.UnitY;
    public float Fov { get; set; } = 45.0f;
    public float Near { get; set; } = 0.1f;
    public float Far { get; set; } = 1000.0f;

    public Listener? Listener { get; set; }

    public override void LateUpdate()
    {
        Listener?.UpdateListener(Transform, (Target - Transform.GlobalPosition).Normalize(), Up);
        base.LateUpdate();
    }

    public virtual Matrix Projection
        => Transformations.PerspectiveProjection(Fov, Window.AspectRatio, Near, Far);

    public virtual Matrix View 
        => Transformations.LookAtMatrix(Transform.GlobalPosition, Target, Up);
}

