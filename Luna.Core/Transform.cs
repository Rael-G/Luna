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
        get => Parent?.GlobalRotation.CombineRotation(Rotation)?? Rotation;
    }

    public Vector3 Scale { get; set; }

    public Vector3 GlobalScale
    {
        get => Parent?.GlobalScale.Scale(Scale)?? Scale;
    }

    public Vector3 Origin { get; set; }

    internal Transform? Parent { get; set; }

    public Vector3 RotationDegrees 
    { 
        get => Rotation.ToDegrees(); 
        set => Rotation = value.ToRadians(); 
    }

    public Vector3 GlobalRotationDegrees 
    { 
        get => GlobalRotation.ToDegrees();
    }

    internal Matrix ModelMatrix()
        =>  Transformations.TranslationMatrix(GlobalPosition) *
            Transformations.TranslationMatrix(Origin) *
            Transformations.RotationMatrix(GlobalRotation.X, Vector3.UnitX) *
            Transformations.RotationMatrix(GlobalRotation.Y, Vector3.UnitY) *
            Transformations.RotationMatrix(GlobalRotation.Z, Vector3.UnitZ) *
            Transformations.TranslationMatrix(-Origin) *
            Transformations.ScaleMatrix(GlobalScale);

}
