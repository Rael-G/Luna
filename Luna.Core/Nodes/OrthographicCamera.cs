using Luna.Maths;

namespace Luna;

public class OrtographicCamera : Node2D, ICamera
{
    public float Left { get; set; } = 0.0f;
    public float Right { get; set; } = 1.0f;
    public float Bottom { get; set; } = 1.0f;
    public float Top { get; set; } = 0.0f;
    public float Near { get; set; } = -10.0f;
    public float Far { get; set; } = 10.0f;

    public Listener? Listener { get; set; }

    public virtual Matrix Projection 
        => Transformations.OrthographicProjection(Left, Right, Bottom, Top, Near, Far);
    

    public virtual Matrix View 
        => Transformations.TranslationMatrix(-Transform.GlobalPosition.ToVector3());
    
    protected override Node? Parent 
    { 
        get => base.Parent; 
        set
        {
            base.Parent = value;
            if (Parent is Node2D parent2D)
                parent2D.Camera = this;
            else if (Parent is Node3D parent3D)
                parent3D.Camera = this;
        } 
    }

    public override void LateUpdate()
    {
        Listener?.UpdateListener(Transform);
        base.LateUpdate();
    }

}