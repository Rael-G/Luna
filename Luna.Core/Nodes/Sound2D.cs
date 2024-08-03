using Luna.Maths;

namespace Luna;

public class Sound2D : Node2D
{
    public string Path
    {
        get => _path;
        set
        {
            _path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), value);
            _player?.Dispose();
            _player = Injector.Get<IAudioPlayerFactory>().Create(_path);
        }
    }

    public float Volume 
    { 
        get => _player.Volume; 
        set => _player.Volume = value; 
    }
    public float Pitch 
    { 
        get => _player.Pitch; 
        set => _player.Pitch = value; 
    }
    public bool Loop 
    { 
        get => _player.Loop; 
        set => _player.Loop = value; 
    }
    public float Speed 
    { 
        get => _player.Speed; 
        set => _player.Speed = value; 
    }
    
    private IAudioPlayer _player = null!;
    private string _path = string.Empty;

    public Sound2D(string path)
    {
        Path = path;
    }

    public void Play()
        => _player.Play();

    public void Pause()
        => _player.Pause();

    public void Stop()
        => _player.Stop();

    public override void LateUpdate()
    {
        _player.Position = Transform.Position.ToVector3();
        base.LateUpdate();
    }

    public override void Dispose(bool disposing)
    {
        _player.Dispose();
        base.Dispose(disposing);
    }

}
