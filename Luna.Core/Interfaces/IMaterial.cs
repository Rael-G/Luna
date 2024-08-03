namespace Luna;

public interface IMaterial : IDisposable
{
    public ModelViewProjection ModelViewProjection { get; set; }
    public void Bind();
}
