using System.Numerics;
using Luna.Core;
using Luna.Maths;

namespace Luna;

public class PerspectiveCamera : Node3D, ICamera
{
    
    protected override Node? Parent 
    { 
        get => base.Parent; 
        set
        {
            base.Parent = value;
            if (Parent is Node3D parent3D)
                parent3D.Camera = this;
        } 
    }

    public Vector3 Target 
    {
        get
        {
            if (_target != null) return (Vector3)_target;

            var front = Vector3.Zero;
            front.X = (float)(Math.Cos(Transform.GlobalRotation.Y) * Math.Cos(Transform.GlobalRotation.Z));
            front.Y = (float)(Math.Sin(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Y) * 
                Math.Cos(Transform.GlobalRotation.Z) - Math.Cos(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Z));
            front.Z = (float)(Math.Cos(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Y) * 
                Math.Cos(Transform.GlobalRotation.Z) + Math.Sin(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Z));
                
            return front;
        }  
        set => _target = value;
    }

    public Vector3 Up { get; set; } = Vector3.UnitY;
    public float Fov { get; set; } = 45.0f;
    public float Near { get; set; } = 0.1f;
    public float Far { get; set; } = 1000.0f;

    public bool IsListener { get; set; }

    private Vector3? _target;

    public override void LateUpdate()
    {
        if (IsListener)
            Utils.SetListener(Transform.GlobalPosition, (Target - Transform.GlobalPosition).Normalize(), Up);
        base.LateUpdate();
    }

    public virtual Matrix Project()
    => Transformations.PerspectiveProjection
        (Fov, Window.AspectRatio, Near, Far) *
        Transformations.LookAtMatrix(Transform.GlobalPosition, Target, Up);
}

