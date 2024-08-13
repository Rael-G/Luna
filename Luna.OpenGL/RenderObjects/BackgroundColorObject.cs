using Silk.NET.OpenGL;

namespace Luna.OpenGL;

internal class BackgroundColorObject(Color data) : RenderObject<Color>
{
    private Color _color = data;

    public override void Draw()
    {
        GL.ClearColor(_color);
        GlErrorUtils.CheckError("Background");
    }

    public override void Update(Color data)
    {
        _color = data;
    }

    public override void Dispose(bool disposing)
    {
    }
}
