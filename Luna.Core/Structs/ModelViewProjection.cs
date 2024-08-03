using System.Numerics;
using Luna.Maths;

namespace Luna;

public struct ModelViewProjection
{
    public Matrix Model { get; set; }
    public Matrix View { get; set; }
    public Matrix Projection { get; set; }
    public Vector3 CameraPosition { get; set; }
}
