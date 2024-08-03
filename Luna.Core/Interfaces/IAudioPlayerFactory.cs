namespace Luna;

public interface IAudioPlayerFactory
{
    IAudioPlayer Create(string path);
}
