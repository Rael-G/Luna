namespace Luna.Core;

public class Host(Node root)
{
    public Node Root { get; set; } = root;

    public void Run()
    {
        Root.InternalConfig();

        Window.EngineWindow.Init();

        Root.InternalAwake();
        Root.InternalStart();

        while (Window.Running)
        {
            Root.InternalEarlyUpdate();
            Root.InternalUpdate();
            Root.InternalLateUpdate();

            Window.EngineWindow.BeginRender();
            Root.Render();
            Window.EngineWindow.EndRender();
        }

        Window.EngineWindow.Close();
    }

}
