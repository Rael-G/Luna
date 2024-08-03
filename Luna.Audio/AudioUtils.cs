using System.Numerics;

namespace Luna.Audio;

public class AudioUtils : IAudioUtils
{
    public void SetListener(Vector3 position, Vector3 velocity, Vector3 front, Vector3 up)
    {
        AudioManager.SetListener(position, velocity, front, up);
    }

    public void SetDistanceModel(DistanceModel distanceModel)
    {
        AudioManager.SetDistanceModel(distanceModel);
    }

    public void SetGlobalVolume(float volume)
    {
        AudioManager.SetGlobalVolume(volume);
    }
}
