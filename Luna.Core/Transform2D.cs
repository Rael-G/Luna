using System.Numerics;
using Luna.Maths;

namespace Luna;

public class Transform2D
{
    public Transform2D()
    {
        Position = Vector2.Zero;
        Rotation = 0;
        Scale = new(1.0f, 1.0f);
    }

    public Transform2D(Vector2 position, float rotation, Vector2 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public Vector2 Position { get; set; }

    public Vector2 GlobalPosition 
    {
        get 
        {
            if (Parent is null) return Position;
            return Parent.GlobalPosition + Position;
        }
    }

    public float Rotation { get; set; }

    public float GlobalRotation 
    { 
        get => Parent?.GlobalRotation + Rotation?? Rotation;
    }

    public Vector2 Scale { get; set; }

    public Vector2 GlobalScale
    {
        get => Parent?.GlobalScale.Scale(Scale)?? Scale;
    }

    internal Transform2D? Parent { get; set; }

    public float RotationDegrees 
    { 
        get => Rotation.ToDegrees(); 
        set => Rotation = value.ToRadians(); 
    }

    public float GlobalRotationDegrees 
    { 
        get => GlobalRotation.ToDegrees();
    }

    public Matrix ModelMatrix()
         => Transformations.TranslationMatrix(GlobalPosition.ToVector3()) *
            Transformations.RotationMatrix(GlobalRotation, Vector3.UnitZ) *
            Transformations.ScaleMatrix(GlobalScale.ToVector3());  
}
