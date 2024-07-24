using System.Numerics;
using Luna.Core;

namespace Luna.Audio;

public class AudioPlayer(string path)  : IAudioPlayer
{
    private readonly Source _source = AudioManager.Get(path);

    public float Volume 
    { 
        get => _source.Volume;
        set => _source.Volume = value;
    }
    
    public float Pitch 
    { 
        get => _source.Pitch;
        set => _source.Pitch = value;
    }

    public bool Loop 
    { 
        get => _source.Loop;
        set => _source.Loop = value; 
    }

    public float Speed 
    { 
        get => _source.Speed;
        set => _source.Speed = value;
    }

    public Vector3 Position 
    { 
        get => _source.Position;
        set => _source.Position = value;
    }

    public void Pause()
        => _source.Pause();
    

    public void Play()
        => _source.Play();
    

    public void Stop()
        => _source.Stop();
    
}
