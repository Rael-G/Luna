namespace Luna;

[Flags]
public enum WindowFlags
{
    None = 0,
    Vsync = 1 << 0,
    BackFaceCulling = 1 << 3
}

public enum CursorMode
{
    Normal = 212993,
    Hidden,
    Disabled
}
