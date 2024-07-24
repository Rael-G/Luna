using Luna.Core;

namespace Luna.Audio;

public class AudioPlayerFactory : IAudioPlayerFactory
{
    public IAudioPlayer Create(string path)
        => new AudioPlayer(path);
    
}
