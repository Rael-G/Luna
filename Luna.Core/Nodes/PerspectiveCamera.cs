using Luna.Maths;

namespace Luna.Core;

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

    public Vector3D Target 
    {
        get
        {
            if (_target != null) return (Vector3D)_target;

            var front = Vector3D.Zero;
            front.X = Math.Cos(Transform.GlobalRotation.Y) * Math.Cos(Transform.GlobalRotation.Z);
            front.Y = Math.Sin(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Y) * 
                Math.Cos(Transform.GlobalRotation.Z) - Math.Cos(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Z);
            front.Z = Math.Cos(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Y) * 
                Math.Cos(Transform.GlobalRotation.Z) + Math.Sin(Transform.GlobalRotation.X) * Math.Sin(Transform.GlobalRotation.Z);
                
            return front;
        }  
        set => _target = value;
    }

    public Vector3D Up { get; set; } = Vector3D.Up;
    public double Fov { get; set; } = 45.0;
    public double Near { get; set; } = 0.1;
    public double Far { get; set; } = 1000.0;

    public Vector3D? _target;

    public virtual Matrix Project()
    => Transformations.PerspectiveProjection
        (Fov, Window.AspectRatio, Near, Far) *
        Transformations.LookAtMatrix(Transform.GlobalPosition, Target, Up);
}

