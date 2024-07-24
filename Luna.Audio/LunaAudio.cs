using Luna.Core;

namespace Luna.Audio;

public static class LunaAudio
{
    public static void AddServices()
    {
        Injector.AddSingleton<IAudioPlayerFactory>(new AudioPlayerFactory());
        Injector.AddSingleton<IAudioUtils>(new AudioUtils());
    }
}
