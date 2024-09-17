using Luna.Core.Events;

namespace Luna;

public class Host(Node root)
{
    public Node Root { get; set; } = root;

    public void Run()
    {
        Tree.Root = Root;
        Time.StartTimer();
        Root.InternalConfig();

        Window.EngineWindow.Init();

        Injector.Get<IWindow>().SetKeyCallback((key, action, mods) 
            => Root.Input(new KeyboardEvent(key, action, mods)));
        Injector.Get<IWindow>().SetMouseCursorPosCallback((x, y) 
            => Root.Input(new MousePositionEvent(x, y)));
        Injector.Get<IWindow>().SetScrollCallback((x, y) 
            => Root.Input(new MouseScrollEvent(x, y)));
        Injector.Get<IWindow>().SetMouseButtonCallback((button, action, mods) 
            => Root.Input(new MouseButtonEvent(button, action, mods)));

       Root.InternalAwake();
       Root.InternalStart();

       Physics.Root = Root;

        while (Window.Running)
        {
            Time.NextFrame();
            Time.SetDeltaTime();
            Root.InternalEarlyUpdate();
            Root.InternalUpdate();
            Root.InternalLateUpdate();
            Physics.FixedUpdate();

            Window.EngineWindow.BeginRender();
            Root.Draw();
            Window.EngineWindow.EndRender();
        }

        Window.EngineWindow.Close();
    }

}
