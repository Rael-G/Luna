namespace Luna.Core;

public abstract class Disposable : IDisposable
{
    protected bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public abstract void Dispose(bool disposing);

    ~Disposable()
    {
        Dispose(false);
    }
}
