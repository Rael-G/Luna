namespace Luna.Core;

public interface IAudioPlayerFactory
{
    IAudioPlayer Create(string path);
}
