namespace Luna;

public static class SystemExtensions
{
    public static void Add<T>(this List<T> list, params T[] args)
    {
        foreach(var arg in args)
            list.Add(arg);
    }
}
