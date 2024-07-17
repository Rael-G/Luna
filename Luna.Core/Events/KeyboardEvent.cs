namespace Luna;

public class KeyboardEvent(Keys key, InputAction action, KeyModifiers mods) : InputEvent
{
    public Keys Key { get; set; } = key;
    public InputAction Action { get; set; } = action;
    public KeyModifiers KeyModifiers { get; set; } = mods;  
}
