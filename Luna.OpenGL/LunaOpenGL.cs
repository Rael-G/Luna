namespace Luna.OpenGL;

public static class LunaOpenGL
{
    public static void AddServices()
    {
        Injector.AddSingleton<IWindow>(new Window());
        Injector.AddSingleton<IRenderer>(new Renderer());
        Injector.AddSingleton<ILightEmitter>(new LightEmitter());
        Injector.AddSingleton<IRenderObjectFactory>(new RenderObjectFactory());
        Injector.AddSingleton<IEngineUtils>(new EngineUtils());
        
        Injector.AddTransient<IStandardMaterial>(() => new StandardMaterial());

        RenderObjectFactory.RegisterCallback<RectangleData>((data) => new RectangleObject(data));
        RenderObjectFactory.RegisterCallback<TextData>((data) => new TextObject(data));
        RenderObjectFactory.RegisterCallback<Color>((data) => new BackgroundColorObject(data));
        RenderObjectFactory.RegisterCallback<EllipseData>((data) => new EllipseObject(data));
        RenderObjectFactory.RegisterCallback<BoxData>((data) => new BoxObject(data));

    }
}
