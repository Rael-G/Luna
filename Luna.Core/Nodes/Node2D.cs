using Luna.Maths;

namespace Luna;

public class Node2D : Node
{
    public Transform2D Transform { get; }

    public Matrix TransformMatrix
    {
        get 
        {
            var ViewProj = Camera?.Project()?? Matrix.Identity(4);
            return ViewProj * Transform.ModelMatrix(); 
        }
    }

    protected override Node? Parent 
    { 
        get => _parent;
        set 
        {
            _parent = value;
            if (Parent is Node2D parent2D)
                Transform.Parent = parent2D.Transform;
            else
                Transform.Parent = null;
        }
    }

    public Node? _parent;

    public virtual OrtographicCamera? Camera 
    { 
        get
        {
            if (_camera is not null)
                return _camera;

            var parent2D = Parent as Node2D;
            return parent2D?.Camera;
        }
        set
        {
            _camera = value;
        } 
    }

    public OrtographicCamera? _camera;

    public Node2D()
    {
        Transform = new();
    }
}
