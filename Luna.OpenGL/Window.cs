using System.Numerics;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal unsafe class Window : IWindow
{
    public string Title 
    { 
        get => _title;
        set
        {
            _title = value;
            Glfw?.SetWindowTitle(WindowHandle, _title);
        } 
    }

    public bool Running 
    { 
        get => Glfw is not null && WindowHandle is not null && !Glfw.WindowShouldClose(WindowHandle) && _running;
        set => _running = value; 
    }

    public Vector2 Size
    {
        get => _windowSize;
        set 
        {
            _windowSize = value;
            GL?.Viewport(0, 0, (uint)_windowSize.X, (uint)_windowSize.Y);
        }
    }

    public bool Vsync 
    {
        get => _vsync;
        set 
        {
            _vsync = value;
            Glfw?.SwapInterval(_vsync? 1 : 0);
        }
    }

    public bool DisableCursor
    {
        get => _disableCursor;
        set 
        {
            _disableCursor = value;
            Glfw?.SetInputMode(WindowHandle, CursorStateAttribute.Cursor, value);   
        } 
    }
    
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Glfw? Glfw;
    public static GL? GL;
#pragma warning restore CA2211 // Non-constant fields should not be visible
    private static WindowHandle* WindowHandle = null!;

    private KeyCallback? _keyCallback;
    private MouseCursorPosCallback? _mouseCursorPosCallback;
    private ScrollCallback? _scrollCallback;

    private string _title = string.Empty;
    private Vector2 _windowSize = new(800, 600);
    private bool _running;
    private bool _vsync;
    private bool _disableCursor;

    public void Init()
    {
        Glfw = Glfw.GetApi();

        if (!Glfw.Init())
            throw new WindowException("GLFW failed to init.");
        
        Glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
        Glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
        Glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

        WindowHandle = Glfw.CreateWindow((int)Size.X, (int)Size.Y, Title, null, null);

        if (WindowHandle is null)
        {
            Glfw.Terminate();
            throw new WindowException("Glfw failed to create window.");
        }

        Glfw.MakeContextCurrent(WindowHandle);

        GL = GL.GetApi(new GlfwContext(Glfw, WindowHandle));
        if (GL is null)
        {
            Glfw.Terminate();
            throw new WindowException("GL failed to load context.");
        }

        Glfw.SetErrorCallback(ErrorCallback);
        Glfw.SetFramebufferSizeCallback(WindowHandle, FrameBufferSizeCallback);

        Running = true;

        Glfw?.SwapInterval(Vsync? 1 : 0);
        Glfw?.SetInputMode(WindowHandle, CursorStateAttribute.Cursor, DisableCursor);
    }

    public void Close()
    {
        Glfw?.DestroyWindow(WindowHandle);
        Glfw?.Terminate();
    }

    public void BeginRender()
    {
        GL?.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL?.ClearColor(Colors.DimGray);
    }
    
    public void EndRender()
    {
        Glfw?.SwapBuffers(WindowHandle);
        Glfw?.PollEvents();
    }

    public InputAction KeyStatus(Keys key)
    {
        if (Glfw is null)
            throw new WindowException("Window was not initialized");


        return (InputAction)Glfw.GetKey(WindowHandle, (Silk.NET.GLFW.Keys)key);
    }

    public void SetKeyCallback(KeyCallback keyCallback)
    {
        _keyCallback = keyCallback;
        Glfw?.SetKeyCallback(WindowHandle, KeyCallback);
    }

    public void SetMouseCursorPosCallback(MouseCursorPosCallback mouseCursorPosCallback)
    {
        _mouseCursorPosCallback = mouseCursorPosCallback;
        Glfw?.SetCursorPosCallback(WindowHandle, MouseCursorPosCallback);
    }

    public void SetScrollCallback(ScrollCallback scrollCallback)
    {
        _scrollCallback = scrollCallback;
        Glfw?.SetScrollCallback(WindowHandle, ScrollCallback);
    }

    private void MouseCursorPosCallback(WindowHandle* window, double xpos, double ypos)
    {
        if (_mouseCursorPosCallback is not null)
            _mouseCursorPosCallback(xpos, ypos);
    }


    private void KeyCallback(WindowHandle* window, Silk.NET.GLFW.Keys key, int scanCode, Silk.NET.GLFW.InputAction action, Silk.NET.GLFW.KeyModifiers mods)
    {
        if (_keyCallback is not null)
            _keyCallback((Keys)key, (InputAction)action, (KeyModifiers)mods);
    }

    private void ScrollCallback(WindowHandle* window, double offsetX, double offsetY)
    {
        if (_scrollCallback is not null)
            _scrollCallback(offsetX, offsetY);
    }

    private void FrameBufferSizeCallback(WindowHandle* window, int width, int height)
        => Size = new Vector2(width, height);
    

    private void ErrorCallback(Silk.NET.GLFW.ErrorCode error, string description)
        => Console.WriteLine($"GLFW error {error}. {description}");
    

}
