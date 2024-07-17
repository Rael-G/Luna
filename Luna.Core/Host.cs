namespace Luna;

public class Host
{
    public Node Root { get; set; }

    private readonly InputHandler InputHandler = _ => {};

    public Host(Node root)
    {
        Root = root;
        InputHandler += Root.Input;
    }

    public void Run()
    {
        Root.Config();

        Window.EngineWindow.Init();

        Injector.Get<IWindow>().SetKeyCallback((key, action, mods) 
            => InputHandler(new KeyboardEvent(key, action, mods)));

       Root.Awake();
       Root.Start();

        while (Window.Running)
        {
            Root.EarlyUpdate();
            Root.Update();
            Root.LateUpdate();
            Physics.FixedUpdate();

            Window.EngineWindow.BeginRender();
            Root.Render();
            Window.EngineWindow.EndRender();
        }

        Window.EngineWindow.Close();
    }

}
