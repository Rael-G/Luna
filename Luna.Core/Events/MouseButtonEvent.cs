namespace Luna;

public class MouseButtonEvent(MouseButton button, InputAction action, KeyModifiers mods) : InputEvent
{
    public MouseButton Button { get; set; } = button;
    public InputAction Action { get; set; } = action;
    public KeyModifiers KeyModifiers { get; set; } = mods;  
}
