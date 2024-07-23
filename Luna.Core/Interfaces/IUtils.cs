using System.Numerics;

namespace Luna.Core;

using FontKey = (string Path, Vector2 Size);

public interface IUtils
{
    Vector2 MeasureTextSize(FontKey font, string text);
}
