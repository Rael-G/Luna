using System.Numerics;
using System.Runtime.Serialization;

namespace Luna;

[Serialize]
public class DirectionalLight
{
    public Vector3 Position { get; internal set; }
    public virtual Vector3 Direction { get; set; } = -Vector3.UnitZ;

    public Color Color { get; set; } = Colors.White;

    [IgnoreDataMember]
    public Vector3 Ambient 
    { 
        get => _ambient * Color.RGB(); 
        set => _ambient = value; 
    } 

    [IgnoreDataMember]
    public Vector3 Diffuse 
    { 
        get => _diffuse * Color.RGB(); 
        set => _diffuse = value; 
    }

    [IgnoreDataMember]
    public Vector3 Specular 
    { 
        get => _specullar * Color.RGB(); 
        set => _specullar = value; 
    }

    [DataMember]
    private Vector3 _ambient = new(0.2f, 0.2f, 0.2f);

    [DataMember]
    private Vector3 _diffuse = new(.9f, .9f, .9f);

    [DataMember]
    private Vector3 _specullar = new(0.5f, 0.5f, 0.5f);
    
}
