namespace Luna;

public static class Keyboard
{
    public static bool KeyDown(Keys key)
        => Window.EngineWindow.KeyStatus(key) == InputAction.Down;

    public static bool KeyUp(Keys key)
        => Window.EngineWindow.KeyStatus(key) == InputAction.Up;
}
