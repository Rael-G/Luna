using System.Numerics;
using Luna.Maths;
using Silk.NET.OpenAL;

namespace Luna.Audio;

public class Source(string path, uint handle, uint buffer) : Disposable
{
    private static readonly AL _al = AudioContext.Al;
    public uint Handle = handle;
    public uint Buffer = buffer;

    public float Volume 
    { 
        get => _volume;
        set
        {
            _volume = value.Clamp(0f, 1f);
            _al.SetSourceProperty(Handle, SourceFloat.Gain, _volume);
        }
    }

    public float Pitch 
    {
        get => +_pitch;
        set
        {
            _pitch = value.Clamp(0.5f, 2f);
            _al.SetSourceProperty(Handle, SourceFloat.Pitch, _pitch);
        }
    }

    public bool Loop 
    { 
        get => _loop;
        set
        {
            _loop = value;
            _al.SetSourceProperty(Handle, SourceBoolean.Looping, _loop);
        }
    }

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = Math.Abs(value);
            _al.SpeedOfSound(_speed);
        }
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            _al.SetSourceProperty(Handle, SourceVector3.Position, _position);
        }
    }

    public float ReferenceDistance
    {
        get => _referenceDistance;
        set
        {
            _referenceDistance = Math.Abs(value);
            _al.SetSourceProperty(Handle, SourceFloat.ReferenceDistance, _referenceDistance);
        }
    }

    public float MaxDistance 
    {
        get => _maxDistance;
        set
        {
            _maxDistance = Math.Abs(value);
            _al.SetSourceProperty(Handle, SourceFloat.MaxDistance, _maxDistance);
        }
    }

    public float RolloffFactor 
    {
        get => _rollofFactor;
        set
        {
            _rollofFactor = Math.Abs(value);
            _al.SetSourceProperty(Handle, SourceFloat.RolloffFactor, _rollofFactor);
        }
    }

    private readonly string _path = path;
    private float _volume = 1f;
    private float _pitch = 1f;
    private bool _loop;
    private float _speed = 1f;
    private Vector3 _position;
    private float _referenceDistance = 1f;
    private float _maxDistance = 1000f;
    private float _rollofFactor = 1f;

    public void Play()
    {
        _al.SourcePlay(Handle);
    }

    public void Stop()
    {
        _al.SourceStop(Handle);
    }

    public void Pause()
    {
        _al.SourcePause(Handle);
    }

    internal void StartUsing()
    {
        AudioManager.StartUsing(_path);
    }

    internal void StopUsing()
    {
        AudioManager.StopUsing(_path);
    }

    public override void Dispose(bool disposing)
    {
        _al.DeleteSource(Handle);
        _al.DeleteBuffer(Buffer);
    }
}
