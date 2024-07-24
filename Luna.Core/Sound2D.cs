using System.Numerics;
using Luna.Maths;

namespace Luna.Core;

public class Sound2D(string path) : Node2D
{
    private IAudioPlayer Player = Injector.Get<IAudioPlayerFactory>().Create(path);

    public string Path
    {
        get => _path;
        set
        {
            _path = value;
            Player.Dispose();
            Player = Injector.Get<IAudioPlayerFactory>().Create(_path);
        }
    }

    public float Volume 
    { 
        get => Player.Volume; 
        set => Player.Volume = value; 
    }
    public float Pitch 
    { 
        get => Player.Pitch; 
        set => Player.Pitch = value; 
    }
    public bool Loop 
    { 
        get => Player.Loop; 
        set => Player.Loop = value; 
    }
    public float Speed 
    { 
        get => Player.Speed; 
        set => Player.Speed = value; 
    }

    private string _path = path;

    public void Play()
        => Player.Play();

    public void Pause()
        => Player.Pause();

    public void Stop()
        => Player.Stop();

    public override void LateUpdate()
    {
        Player.Position = Transform.Position.ToVector3();
        base.LateUpdate();
    }

    public override void Dispose(bool disposing)
    {
        Player.Dispose();
        base.Dispose(disposing);
    }

}
