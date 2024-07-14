using Luna.Maths;

namespace Luna.Core;

public class Transform2D
{
    public Transform2D()
    {
        Position = Vector2D.Zero;
        Rotation = 0;
        Scale = new(1.0, 1.0);
    }

    public Transform2D(Vector2D position, double rotation, Vector2D scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public Vector2D Position { get; set; }

    public Vector2D GlobalPosition 
    {
        get 
        {
            if (Parent is null) return Position;
            return Parent.GlobalPosition + Position;
        }
    }

    public double Rotation { get; set; }

    public double GlobalRotation 
    { 
        get => Parent?.GlobalRotation + Rotation?? Rotation;
    }

    public Vector2D Scale { get; set; }

    public Vector2D GlobalScale
    {
        get => Parent?.GlobalScale.Scale(Scale)?? Scale;
    }

    internal Transform2D? Parent { get; set; }

    public double RotationDegrees 
    { 
        get => Rotation.ToDegrees(); 
        set => Rotation = value.ToRadians(); 
    }

    public double GlobalRotationDegrees 
    { 
        get => GlobalRotation.ToDegrees();
    }

    internal Matrix ModelMatrix()
         => Transformations.TranslationMatrix(GlobalPosition) *
            Transformations.RotationMatrix(GlobalRotation, Vector3D.Backward) *
            Transformations.ScaleMatrix(GlobalScale);  
}
