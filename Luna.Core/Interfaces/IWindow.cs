using System.Numerics;

namespace Luna;

public interface IWindow
{
    WindowFlags Flags { get; set; }
    CursorMode CursorMode { get; set; }
    DepthMode DepthMode { get; set; }
    string Title { get; set;}
    bool Running { get; set; }
    Vector2 Size { get; set; }

    void Init();
    void Close();
    void BeginRender();
    void EndRender();
    InputAction KeyStatus(Keys key);
    void SetKeyCallback(KeyCallback keyCallback);
    void SetMouseCursorPosCallback(MouseCursorPosCallback mouseCursorPosCallback);
    void SetScrollCallback(ScrollCallback scrollCallback);
    void SetMouseButtonCallback(MouseButtonCallback mouseButtonCallback);
}