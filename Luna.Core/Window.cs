﻿using System.Numerics;
using Luna.Maths;

namespace Luna;

public static class Window
{
    public static WindowFlags Flags { get => EngineWindow.Flags; set => EngineWindow.Flags = value; }

    public static readonly IWindow EngineWindow = Injector.Get<IWindow>();
    
    public static string Title { get => EngineWindow.Title; set => EngineWindow.Title = value; }

    public static bool Running { get => EngineWindow.Running; set => EngineWindow.Running = value; }

    public static Vector2 Size { get => EngineWindow.Size; set => EngineWindow.Size = value; }

    public static Vector2 VirtualSize { get; set; } = Size;

    public static Vector3 VirtualCenter { get => (VirtualSize / 2).ToVector3(); }

    public static Vector2 VirtualScale { get => VirtualSize / Size; }

    public static float AspectRatio { get => Size.X / Size.Y; }

    public static CursorMode CursorMode { get => EngineWindow.CursorMode; set => EngineWindow.CursorMode = value; }

    public static DepthMode DepthMode { get => EngineWindow.DepthMode; set => EngineWindow.DepthMode = value; }
}
