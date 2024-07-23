using System.Numerics;
using Box2DSharp.Collision.Shapes;

namespace Luna.Box2D;

public static class Box2DExtensions
{
    public static Vector2 ToPixels(this Vector2 sizeInMeters)
        => sizeInMeters * WorldManager.PixelsPerMeter;

    public static Vector2 ToMeters(this Vector2 sizeInPixels)
        => sizeInPixels / WorldManager.PixelsPerMeter;

    public static float ToPixels(this float sizeInMeters)
        => sizeInMeters * WorldManager.PixelsPerMeter;

    public static float ToMeters(this float sizeInPixels)
        => sizeInPixels / WorldManager.PixelsPerMeter;
}
