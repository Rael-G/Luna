using Luna.Maths;

namespace Luna.Core;

public class Transform3D
{
    public Transform3D()
    {
        Position = Vector3D.Zero;
        Rotation = Vector3D.Zero;
        Scale = new(1.0, 1.0, 1.0);
    }

    public Transform3D(Vector3D position, Vector3D rotation, Vector3D scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public Vector3D Position { get; set; }

    public Vector3D GlobalPosition 
    { 
        get 
        {
            if (Parent is null) return Position;
            return Parent.GlobalPosition + Position;
        }
    }

    public Vector3D Rotation { get; set; }

    public Vector3D GlobalRotation 
    { 
        get => Parent?.GlobalRotation.CombineRotation(Rotation)?? Rotation;
    }

    public Vector3D Scale { get; set; }

    public Vector3D GlobalScale
    {
        get => Parent?.GlobalScale.Scale(Scale)?? Scale;
    }

    internal Transform3D? Parent { get; set; }

    public Vector3D RotationDegrees 
    { 
        get => Rotation.ToDegrees(); 
        set => Rotation = value.ToRadians(); 
    }

    public Vector3D GlobalRotationDegrees 
    { 
        get => GlobalRotation.ToDegrees();
    }

    internal Matrix ModelMatrix()
        =>  Transformations.TranslationMatrix(GlobalPosition) *
            Transformations.RotationMatrix(GlobalRotation.X, Vector3D.Right) *
            Transformations.RotationMatrix(GlobalRotation.Y, Vector3D.Up) *
            Transformations.RotationMatrix(GlobalRotation.Z, Vector3D.Backward) *
            Transformations.ScaleMatrix(GlobalScale);

}
