using System.Numerics;
using Luna.Maths;

namespace Luna;

public class Transform
{
    public Transform()
    {
        Position = Vector3.Zero;
        Rotation = Vector3.Zero;
        Scale = new(1.0f, 1.0f, 1.0f);
    }

    public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public Vector3 Position { get; set; }

    public Vector3 GlobalPosition 
    { 
        get 
        {
            if (Parent is null) return Position - Origin;
            return Parent.GlobalPosition + Position - Origin;
        }
    }

    public Vector3 Rotation { get; set; }

    public Vector3 GlobalRotation 
    { 
        get => Parent?.GlobalRotation + Rotation?? Rotation;
    }

    public Vector3 Scale { get; set; }

    public Vector3 GlobalScale
    {
        get => Parent?.GlobalScale.Scale(Scale)?? Scale;
    }

    public Vector3 Origin { get; set; }

    internal Transform? Parent { get; set; }

    public Vector3 EulerAngles 
    { 
        get => Rotation.ToDegrees(); 
        set => Rotation = value.ToRadians(); 
    }

    public Vector3 GlobalEulerAngles 
    { 
        get => GlobalRotation.ToDegrees();
    }

    public Quaternion Quaternion 
    { 
        get => Rotation.ToQuaternion(); 
        set => Rotation = value.ToVector3(); 
    }

    public Quaternion GlobalQuaternion 
    { 
        get => GlobalRotation.ToQuaternion();
    }

    internal Matrix4x4 ModelMatrix()
        =>  GlobalScale.CreateScale() *
            (-Origin).CreateTranslation() *
            GlobalQuaternion.ToMatrix4x4() *
            (Origin).CreateTranslation() *
            GlobalPosition.CreateTranslation();

}
