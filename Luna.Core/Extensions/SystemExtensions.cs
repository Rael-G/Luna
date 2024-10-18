namespace Luna;

public static class SystemExtensions
{
    public static void Add<T>(this IEnumerable<T> list, params T[] args)
    {
        foreach(var arg in args)
            list.Add(arg);
    }
}
