namespace Luna;

public abstract class Disposable : IDisposable
{
    protected bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing)
    {
        _disposed = true;
    }

    ~Disposable()
    {
        Dispose(false);
    }
}
