namespace Luna.OpenGl;

public static class LunaOpenGL
{
    public static void AddServices()
    {
        Injector.AddSingleton<IWindow>(new Window());
        Injector.AddSingleton<IRenderMap>(new RenderMap());
        Injector.AddSingleton<IRenderObjectFactory>(new RenderObjectFactory());

        RenderObjectFactory.RegisterCallback<RectangleData>((data) => new RectangleObject(data));
        RenderObjectFactory.RegisterCallback<TextData>((data) => new TextObject(data));
    }
}
