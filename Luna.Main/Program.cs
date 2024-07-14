using Luna.Core;
using Luna.Maths;
using Luna.Engine.OpenGl;

internal class Program
{
    private static void Main(string[] args)
    {
        OpenGl.AddServices();
        var root = new Root();
        var camera = new OrtographicCamera(){
            Left = 0.0,
            Right = Window.Size.X,
            Top = 0.0,
            Bottom = Window.Size.Y
        };
        
        var rect = new Rectangle
        {
            Width = Window.Size.X / 2,
            Height = Window.Size.Y / 2,
            Color = Colors.Red
        };
        var position = rect.Transform.Position;
        position.X = 400;
        position.Y = 300;
        rect.Transform.Position = position;
        var a = Vector2D.Zero + Vector2D.Left;
        root.AddChild(camera);
        root.AddChild(rect);

        var host = new Host(root);
        host.Run();
    }
}

public class Root : Node2D
{
    protected override void Config()
    {
        Window.Title = "Hello Rectangle!";
        Window.Size = new(800, 600);
    }

    protected override void Update()
    {
        if (Input.KeyboardKeyDown(Keys.Escape))
            Window.Running = false;
    }
}