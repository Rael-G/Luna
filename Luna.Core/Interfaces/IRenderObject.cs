namespace Luna;

public interface IRenderObject : IDisposable
{
    void Draw();

    void Draw(IMaterial material);

    int Priority { get; }
}
