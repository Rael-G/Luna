using System.Numerics;
using Luna.Maths;
using Silk.NET.OpenAL;

namespace Luna.Audio;

public class Source
{
    private static readonly AL _al = AudioContext.Al;
    
    public uint Handle;
    public uint Buffer;

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

    private float _volume;
    private float _pitch;
    private bool _loop;
    private float _speed;
    private Vector3 _position;

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

    public void Free()
    {
        _al.DeleteSource(Handle);
        _al.DeleteBuffer(Buffer);
    }

    ~Source()
    {
        Free();
    }
}
