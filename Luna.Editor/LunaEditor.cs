namespace Luna.Editor;

public static class LunaEditor
{
    public static void AddServices()
    {
        Injector.AddSingleton<IEditor>(new ProjectHandler());
    }
}
