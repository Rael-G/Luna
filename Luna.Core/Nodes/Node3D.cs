using Luna.Maths;

namespace Luna;

public class Node3D : Node
{
    public Transform3D Transform { get; set; }

    public ModelViewProjection MatrixMVP
    {
        get => new()
        {
            Projection = Camera?.Projection?? Matrix.Identity(4),
            View = Camera?.View?? Matrix.Identity(4),
            Model = Transform.ModelMatrix()
        };
    }

    public virtual ICamera? Camera 
    { 
        get
        {
            if (_camera is not null)
                return _camera;

            var parent3D = Parent as Node3D;
            return parent3D?.Camera;
        }
        set
        {
            _camera = value;
        } 
    }

    protected override Node? Parent 
    { 
        get => _parent;
        set 
        {
            _parent = value;
            if (Parent is Node3D parent3D)
                Transform.Parent = parent3D.Transform;
            else
                Transform.Parent = null;
        }
    }

    public Node? _parent;

    public ICamera? _camera;

    public Node3D()
    {
        Transform = new();
    }
}
