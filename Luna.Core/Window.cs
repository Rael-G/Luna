using Luna.Maths;

namespace Luna.Core;

public static class Window
{
    internal static readonly IWindow EngineWindow = Injector.Get<IWindow>()?? 
        throw new InjectionException("IWindow Service was not added to the Injector.");
    
    public static string Title { get => EngineWindow.Title; set => EngineWindow.Title = value; }

    public static bool Running { get => EngineWindow.Running; set => EngineWindow.Running = value; }

    public static Vector2D Size { get => EngineWindow.Size; set => EngineWindow.Size = value; }

    public static double AspectRatio { get => Size.X / Size.Y; }

    public static bool Vsync { get => EngineWindow.Vsync; set => EngineWindow.Vsync = value; }

    public static bool DisableCursor { get => EngineWindow.DisableCursor; set => EngineWindow.DisableCursor = value; }
}
