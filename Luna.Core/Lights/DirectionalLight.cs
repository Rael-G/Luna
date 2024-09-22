using System.Numerics;

namespace Luna;

public class DirectionalLight
{
    public Vector3 Position { get; internal set; }
    public virtual Vector3 Direction { get; set; } = -Vector3.UnitZ;

    public Color Color { get; set; } = Colors.White;

    public Vector3 Ambient 
    { 
        get => _ambient * Color.RGB(); 
        set => _ambient = value; 
    } 

    public Vector3 Diffuse 
    { 
        get => _diffuse * Color.RGB(); 
        set => _diffuse = value; 
    }

    public Vector3 Specular 
    { 
        get => _specullar * Color.RGB(); 
        set => _specullar = value; 
    }

    private Vector3 _ambient = new(0.2f, 0.2f, 0.2f);
    private Vector3 _diffuse = new(.9f, .9f, .9f);
    private Vector3 _specullar = new(0.5f, 0.5f, 0.5f);
    
}
