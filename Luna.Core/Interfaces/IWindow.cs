using System.Numerics;

namespace Luna;

public interface IWindow
{
    string Title { get; set;}
    bool Running { get; set; }
    Vector2 Size { get; set; }
    bool Vsync { get; set; }
    bool CursorHidden { get; set; }

    void Init();
    void Close();
    void BeginRender();
    void EndRender();
    InputAction KeyStatus(Keys key);
    void SetKeyCallback(KeyCallback keyCallback);
    void SetMouseCursorPosCallback(MouseCursorPosCallback mouseCursorPosCallback);
    void SetScrollCallback(ScrollCallback scrollCallback);
    
}