namespace Luna.Core;

public static class Input
{
    public static bool KeyboardKeyDown(Keys key)
        => Window.EngineWindow.KeyStatus(key) == InputAction.Down;

    public static bool KeyboardKeyUp(Keys key)
        => Window.EngineWindow.KeyStatus(key) == InputAction.Up;
}
