using System.Numerics;
using Luna.Maths;

namespace Luna;

public static class Window
{
    public static readonly IWindow EngineWindow = Injector.Get<IWindow>();
    
    public static string Title { get => EngineWindow.Title; set => EngineWindow.Title = value; }

    public static bool Running { get => EngineWindow.Running; set => EngineWindow.Running = value; }

    public static Vector2 Size { get => EngineWindow.Size; set => EngineWindow.Size = value; }

    public static Vector2 VirtualSize { get; set; } = Size;

    public static Vector3 VirtualCenter { get => (VirtualSize / 2).ToVector3(); }

    public static Vector2 VirtualScale { get => VirtualSize / Size; }

    public static float AspectRatio { get => Size.X / Size.Y; }

    public static bool Vsync { get => EngineWindow.Vsync; set => EngineWindow.Vsync = value; }

    public static bool DisableCursor { get => EngineWindow.DisableCursor; set => EngineWindow.DisableCursor = value; }
}
