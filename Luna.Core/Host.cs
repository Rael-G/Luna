namespace Luna;

public class Host
{
    public Node Root { get; set; }

    private readonly NodeCallback ConfigHandler = () => {};
    private readonly NodeCallback AwakeHandler = () => {};
    private readonly NodeCallback StartHandler = () => {};
    private readonly NodeCallback EarlyUpdateHandler = () => {};
    private readonly NodeCallback UpdateHandler = () => {};
    private readonly NodeCallback LateUpdateHandler = () => {};
    private readonly NodeCallback RenderHandler = () => {};
    private readonly InputHandler InputHandler = _ => {};

    public Host(Node root)
    {
        Root = root;

        ConfigHandler += Root.Config;
        AwakeHandler += Root.Awake;
        StartHandler += Root.Start;
        EarlyUpdateHandler += Root.EarlyUpdate;
        UpdateHandler += Root.Update;
        LateUpdateHandler += Root.LateUpdate;
        RenderHandler += Root.Render;
        InputHandler += Root.Input;
    }

    public void Run()
    {
        ConfigHandler();

        Window.EngineWindow.Init();

        Injector.Get<IWindow>().SetKeyCallback((key, action, mods) 
            => InputHandler(new KeyboardEvent(key, action, mods)));

       AwakeHandler();
       StartHandler();

        while (Window.Running)
        {
            EarlyUpdateHandler();
            UpdateHandler();
            LateUpdateHandler();
            Physics.FixedUpdate();

            Window.EngineWindow.BeginRender();
            RenderHandler();
            Window.EngineWindow.EndRender();
        }

        Window.EngineWindow.Close();
    }

}
