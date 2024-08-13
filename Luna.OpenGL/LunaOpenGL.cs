namespace Luna.OpenGL;

public static class LunaOpenGL
{
    private const string VertexName = "StandardVertexShader.glsl";
    private const string FragmentName = "StandardFragmentShader.glsl";

    private static readonly ShaderSource[] DefaultShader = 
    [
        new()
        {
            Name = VertexName,
            Path = ProgramShader.DefaultShaderPath(VertexName),
            ShaderType = ShaderType.VertexShader
        },
        new()
        {
            Name = FragmentName,
            Path = ProgramShader.DefaultShaderPath(FragmentName),
            ShaderType = ShaderType.FragmentShader
        }
    ];

    public static void AddServices()
    {
        Injector.AddSingleton<IWindow>(new Window());
        Injector.AddSingleton<IRenderer>(new Renderer());
        Injector.AddSingleton<ILightEmitter>(new LightEmitter());
        Injector.AddSingleton<IRenderObjectFactory>(new RenderObjectFactory());
        Injector.AddSingleton<IEngineUtils>(new EngineUtils());
        Injector.AddSingleton(DefaultShader);
        
        Injector.AddTransient<IStandardMaterial>(() => new StandardMaterial());

        RenderObjectFactory.RegisterCallback<RectangleData>((data) => new RectangleObject(data));
        RenderObjectFactory.RegisterCallback<TextData>((data) => new TextObject(data));
        RenderObjectFactory.RegisterCallback<Color>((data) => new BackgroundColorObject(data));
        RenderObjectFactory.RegisterCallback<EllipseData>((data) => new EllipseObject(data));
        RenderObjectFactory.RegisterCallback<BoxData>((data) => new BoxObject(data));
        RenderObjectFactory.RegisterCallback<ModelData>((data) => new Model(data));
        RenderObjectFactory.RegisterCallback<PostProcessorData>((data) => new PostProcessor(data));

    }
}
