using Luna.Core;

namespace Luna.Audio;

public class AudioPlayerFactory
{
    public IAudioPlayer Create(string path)
        => new AudioPlayer(path);
    
}
