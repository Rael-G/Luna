using Luna.Core;

namespace Luna.Engine.OpenGl;

public static class OpenGl
{
    public static void AddServices()
    {
        Injector.Add<IWindow>(new Window());
        Injector.Add<IRenderMap>(new RenderMap());
        Injector.Add<IRenderObjectFactory>(new RenderObjectFactory());
    }
}
