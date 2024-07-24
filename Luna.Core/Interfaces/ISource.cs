using System.Numerics;

namespace Luna.Core;

public interface IAudioPlayer : IDisposable
{
    float Volume { get; set; }
    float Pitch { get; set; }
    bool Loop { get; set; }
    float Speed { get; set; }
    Vector3 Position { get; set; }
    float ReferenceDistance { get; set; }
    float MaxDistance { get; set; }
    float RolloffFactor { get; set; }

    void Play();
    void Stop();
    void Pause();
}
