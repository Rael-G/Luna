using System.Numerics;

namespace Luna;

public interface ICamera
{
    Matrix4x4 Projection { get; }
    Matrix4x4 View { get; }
}



