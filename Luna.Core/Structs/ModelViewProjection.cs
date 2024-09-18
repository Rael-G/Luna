using System.Numerics;
using Luna.Maths;

namespace Luna;

public struct ModelViewProjection
{
    public Matrix4x4 Model { get; set; }
    public Matrix4x4 View { get; set; }
    public Matrix4x4 Projection { get; set; }
    public Vector3 CameraPosition { get; set; }
}
