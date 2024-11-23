namespace Luna;

public static class MainThreadDispatcher
{
    private static readonly Queue<Action> _mainThreadQueue = new Queue<Action>();
    private static readonly object _lock = new object();

    public static void ExecutePending()
    {
        lock (_lock)
        {
            while (_mainThreadQueue.Count > 0)
            {
                var action = _mainThreadQueue.Dequeue();
                action.Invoke();
            }
        }
    }

    public static Task AwaitMainThread()
    {
        var tcs = new TaskCompletionSource<bool>();
        Enqueue(() => tcs.SetResult(true));
        return tcs.Task;
    }

    private static void Enqueue(Action action)
    {
        lock (_lock)
        {
            _mainThreadQueue.Enqueue(action);
        }
    }
}
