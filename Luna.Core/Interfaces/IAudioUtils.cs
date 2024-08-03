using System.Numerics;

namespace Luna;

public interface IAudioUtils
{
    void SetListener(Vector3 position, Vector3 velocity, Vector3 front, Vector3 up);

    void SetDistanceModel(DistanceModel distanceModel);

    void SetGlobalVolume(float volume);
}
