namespace Luna;

public delegate void KeyCallback(Keys key, InputAction action, KeyModifiers mods);
public delegate void MouseCursorPosCallback(double xpos, double ypos);
public delegate void ScrollCallback(double xoffset, double yoffset);
public delegate void InputHandler(InputEvent inputEvent);
public delegate void NodeCallback();
