using System.Numerics;
using Luna.Maths;
using Silk.NET.OpenAL;

namespace Luna.Audio;

public static class AudioManager
{
    private static readonly Dictionary<string, Source> Sources = [];
    private static readonly Dictionary<string, int> Counters = [];

    private static readonly AL _al = AudioContext.Al;

    public static Source Get(string path)
    {
        if (Sources.TryGetValue(path, out var source))
            return source;

        uint buffer = _al.GenBuffer();

        try
        {
            byte[] audioData = LoadWave(path, out var channels, out var bits, out var sampleRate);
            _al.BufferData(buffer, GetSoundFormat(channels, bits), audioData, sampleRate);
        }
        catch(Exception e)
        {
            throw new ResourceException($"Failed to load audio source from path: {path}", e);
        }
        uint handle = _al.GenSource();
        _al.SetSourceProperty(handle, SourceInteger.Buffer, buffer);
        Sources[path] = new(path, handle, buffer);
        return Sources[path];
    }

    public unsafe static void SetListener(Vector3 position, Vector3 velocity, Vector3 front, Vector3 up)
    {
        _al.SetListenerProperty(ListenerVector3.Position, position);
        _al.SetListenerProperty(ListenerVector3.Velocity, velocity);
        fixed (float* orientationPtr = new float[]{ front.X, front.Y, front.Z, up.X, up.Y, up.Z })
        {
            _al.SetListenerProperty(ListenerFloatArray.Orientation, orientationPtr);
        }
    }

    public static void SetDistanceModel(DistanceModel distanceModel)
    {
        _al.DistanceModel((Silk.NET.OpenAL.DistanceModel)distanceModel);
    }

    public static void SetGlobalVolume(float volume)
    {
        _al.SetListenerProperty(ListenerFloat.Gain, volume.Clamp(0f, 1f));
    }

    public static void StartUsing(string path)
    {
        if (!Counters.TryGetValue(path, out _))
            Counters.Add(path, 0);

        Counters[path]++;
    }

    public static void StopUsing(string path)
    {
        if (!Counters.TryGetValue(path, out _))
            return;

        int count = --Counters[path];

        if (count <= 0)
        {
            Sources[path].Dispose();
            Counters.Remove(path);
            Sources.Remove(path);
        }
    }

    private static byte[] LoadWave(string path, out int channels, out int bits, out int rate)
    {
        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream);

        var signature = new string(reader.ReadChars(4));
        if (signature != "RIFF")
            throw new NotSupportedException("Unsuported format.");

        reader.ReadInt32();
        var format = new string(reader.ReadChars(4));
        if (format != "WAVE")
            throw new NotSupportedException("Unsuported format.");

        var formatSignature = new string(reader.ReadChars(4));
        if (formatSignature != "fmt ")
            throw new NotSupportedException("Unsuported format.");

        var formatChunkSize = reader.ReadInt32();
        var audioFormat = reader.ReadInt16();
        channels = reader.ReadInt16();
        rate = reader.ReadInt32();
        reader.ReadInt32(); // Byte rate
        reader.ReadInt16(); // Block align
        bits = reader.ReadInt16();

        if (audioFormat != 1)
            throw new NotSupportedException("Unsuported audio format.");

        var dataSignature = new string(reader.ReadChars(4));
        if (dataSignature != "data")
            throw new NotSupportedException("Unsuported format.");

        var dataSize = reader.ReadInt32();
        return reader.ReadBytes(dataSize);
    }

    private static BufferFormat GetSoundFormat(int channels, int bits)
    {
        return (channels, bits) switch
        {
            (1, 8) => BufferFormat.Mono8,
            (1, 16) => BufferFormat.Mono16,
            (2, 8) => BufferFormat.Stereo8,
            (2, 16) => BufferFormat.Stereo16,
            _ => throw new NotSupportedException("Unsuported sound format."),
        };
    }
}
