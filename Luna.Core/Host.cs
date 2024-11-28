namespace Luna;

public static class Host
{
    public static void CreateWindow()
    {
        Window.EngineWindow.Init();
    }
    
    public static void Run(Node root)
    {
        Tree.Root = root;
        
        Injector.Get<IWindow>().SetKeyCallback((key, action, mods) 
            => Tree.Root.Input(new KeyboardEvent(key, action, mods)));
        Injector.Get<IWindow>().SetMouseCursorPosCallback((x, y) 
            => Tree.Root.Input(new MousePositionEvent(x, y)));
        Injector.Get<IWindow>().SetScrollCallback((x, y) 
            => Tree.Root.Input(new MouseScrollEvent(x, y)));
        Injector.Get<IWindow>().SetMouseButtonCallback((button, action, mods) 
            => Tree.Root.Input(new MouseButtonEvent(button, action, mods)));

       Tree.Root.InternalAwake();
       Tree.Root.InternalStart();

       Time.StartTimer();

        while (Window.Running)
        {
            Time.NextFrame();
            Time.SetDeltaTime();
            Tree.Root.InternalEarlyUpdate();
            Tree.Root.InternalUpdate();
            Tree.Root.InternalLateUpdate();
            Physics.FixedUpdate();

            MainThreadDispatcher.ExecutePending();

            Window.EngineWindow.BeginRender();
            Tree.Root.Draw();
            Injector.Get<IRenderer>().DrawQueue();
            Window.EngineWindow.EndRender();
        }

        Window.EngineWindow.Close();
    }

}
