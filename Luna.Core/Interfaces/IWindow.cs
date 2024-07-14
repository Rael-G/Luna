using Luna.Maths;

namespace Luna.Core;

public delegate void KeyCallback(Keys key, InputAction action, KeyModifiers mods);
public delegate void MouseCursorPosCallback(double xpos, double ypos);
public delegate void ScrollCallback(double xoffset, double yoffset);

public interface IWindow
{
    string Title { get; set;}
    bool Running { get; set; }
    Vector2D Size { get; set; }
    bool Vsync { get; set; }
    bool DisableCursor { get; set; }

    void Init();
    void Close();
    void BeginRender();
    void EndRender();
    InputAction KeyStatus(Keys key);
    void SetKeyCallback(KeyCallback keyCallback);
    void SetMouseCursorPosCallback(MouseCursorPosCallback mouseCursorPosCallback);
    void SetScrollCallback(ScrollCallback scrollCallback);
    
}