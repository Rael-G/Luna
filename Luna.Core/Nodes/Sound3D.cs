namespace Luna.Core;

public class Sound3D(string path) : Node3D
{
    readonly IAudioPlayer Player = Injector.Get<IAudioPlayerFactory>().Create(path);

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

    public void Play()
        => Player.Play();

    public void Pause()
        => Player.Pause();

    public void Stop()
        => Player.Stop();

    public override void LateUpdate()
    {
        Player.Position = Transform.Position;
        base.LateUpdate();
    }
}