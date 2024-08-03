using Luna.Maths;

namespace Luna;

public interface ICamera
{
    public abstract Matrix Projection { get; }
    public abstract Matrix View { get; }

}



