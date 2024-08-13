namespace Luna;

[Flags]
public enum WindowFlags
{
    None = 0,
    Vsync = 1 << 0,
    CursorHidden = 1 << 1,
    BackFaceCulling = 1 << 2
}
