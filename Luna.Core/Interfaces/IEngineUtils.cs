using System.Numerics;

namespace Luna.Core;

using FontKey = (string Path, Vector2 Size);

public interface IEngineUtils
{
    Vector2 MeasureTextSize(FontKey font, string text);
}
