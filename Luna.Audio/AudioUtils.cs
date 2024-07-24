using System.Numerics;
using Luna.Core;

namespace Luna.Audio;

public class AudioUtils : IAudioUtils
{
    public void SetListener(Vector3 position, Vector3 front, Vector3 up)
    {
        AudioManager.SetListener(position, front, up);
    }
}
