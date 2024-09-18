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
    public float Fov { get; set; } = (45.0f).ToRadians();
    public float Near { get; set; } = 0.1f;
    public float Far { get; set; } = 1000.0f;

    public Listener? Listener { get; set; }

    public override void LateUpdate()
    {
        Listener?.UpdateListener(Transform, (Target - Transform.GlobalPosition).Normalize(), Up);
        base.LateUpdate();
    }

    public virtual Matrix4x4 Projection
        => Matrix4x4.CreatePerspectiveFieldOfView(Fov, Window.AspectRatio, Near, Far);

    public virtual Matrix4x4 View 
        => Matrix4x4.CreateLookAt(Transform.GlobalPosition, Target, Up);
}

