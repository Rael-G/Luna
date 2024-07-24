using Silk.NET.OpenGL;

namespace Luna.OpenGl;

internal class BackgroundColorObject(Color data) : RenderObject<Color>
{
    private static readonly GL _gl = Window.GL?? throw new WindowException("Window.Gl is null.");

    private Color _color = data;

    public override void Render()
    {
        _gl.ClearColor(_color);
    }

    public override void Update(Color data)
    {
        _color = data;
    }

    public override void Free()
    {
       
    }
}
