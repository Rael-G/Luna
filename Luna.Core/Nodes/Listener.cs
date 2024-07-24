using System.Numerics;
using Luna.Maths;

namespace Luna.Core;

public class Listener
{
    public Vector3 Velocity { get; set; }

    public float Volume 
    { 
        get => _volume; 
        set
        {
            _volume = value;
            Utils.SetGlobalVolume(_volume);
        }
    }
    public DistanceModel DistanceModel 
    { 
        get => _distanceModel;
        set
        {
            _distanceModel = value;
            Utils.SetDistanceModel(_distanceModel);
        }
    } 

    private float _volume;
    private DistanceModel _distanceModel = DistanceModel.None;

    public void UpdateListener(Transform2D transform)
    {
        Utils.SetListener(transform.GlobalPosition.ToVector3(), Velocity, -Vector3.UnitZ, Vector3.UnitY);
    }

    public void UpdateListener(Transform3D transform, Vector3 front, Vector3 up)
    {
        Utils.SetListener(transform.GlobalPosition, Velocity, front, up);
    }
}
