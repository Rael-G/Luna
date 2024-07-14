using Luna.Maths;

namespace Luna.Core;

public class OrtographicCamera : Node2D, ICamera
{
    public double Left { get; set; } = -1.0;
    public double Right { get; set; } = 1.0;
    public double Bottom { get; set; } = -1.0;
    public double Top { get; set; } = 1.0;
    public double Near { get; set; } = -1.0;
    public double Far { get; set; } = 10.0;

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

    public virtual Matrix Project()
    {
        var proj = Transformations.OrthographicProjection
            (Left, Right, Bottom, Top, Near, Far);
        var view = Transformations.TranslationMatrix(-Transform.GlobalPosition);
        return proj * view;
    }
            
}