using System.Numerics;

namespace Luna.Core;

using FontKey = (string Path, Vector2 Size);

public static class Utils
{
    public static Vector2 MeasureTextSize(FontKey font, string text)
        => Injector.Get<IEngineUtils>().MeasureTextSize(font, text);
    
    public static void SetListener(Vector3 position, Vector3 front, Vector3 up)
        => Injector.Get<IAudioUtils>().SetListener(position, front, up);
}
