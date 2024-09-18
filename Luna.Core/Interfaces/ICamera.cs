using System.Numerics;
using Luna.Maths;

namespace Luna;

public interface ICamera
{
    public abstract Matrix4x4 Projection { get; }
    public abstract Matrix4x4 View { get; }
}



