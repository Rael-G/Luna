using System.Numerics;
using Luna.Core;

namespace Luna.Audio;

public class AudioPlayer  : Disposable, IAudioPlayer
{
    private readonly Source _source;

    public AudioPlayer(string path)
    {
        _source = AudioManager.Get(path);
        _source.StartUsing();
    }

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

    public float ReferenceDistance
    {
        get =>_source.ReferenceDistance;
        set => _source.ReferenceDistance = value;
    }

    public float MaxDistance 
    {
        get =>_source.MaxDistance;
        set => _source.MaxDistance = value;
    }

    public float RolloffFactor 
    {
        get =>_source.RolloffFactor;
        set => _source.RolloffFactor = value;
    }

    public void Pause()
        => _source.Pause();
    

    public void Play()
        => _source.Play();
    

    public void Stop()
        => _source.Stop();

    public override void Dispose(bool disposing)
        => _source.StopUsing();
}
