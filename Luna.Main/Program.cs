using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Luna;
using Luna.Box2D;
using Luna.Core;
using Luna.OpenGl;

internal class Program
{
    private static void Main(string[] args)
    {
        LunaOpenGL.AddServices();
        var root = new Root();
        
        
        var host = new Host(root);
        host.Run();
    }
}

public class Root : Node2D
{
    Label label;
    public override void Config()
    {
        Window.Title = "Hello Rectangle!";
        Window.Size = new(800, 600);
        base.Config();
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        var camera = new OrtographicCamera(){
            Left = 0.0f,
            Right = Window.Size.X,
            Bottom = Window.Size.Y,
            Top = 0.0f 
        };
        AddChild(camera);
        var rect = new Rectangle
        {
            Size = new(400, 300),
            Color = Colors.Red
        };

        rect.Transform.Position = new Vector2{ X = 400, Y = 297 };

        label = new Label
        {
            Text = "Hello, World!",
            Path = Directory.GetCurrentDirectory() + "/Assets/fonts/OpenSans-Regular.ttf",
            FlipV = true,
            CenterH = true,
            CenterV = true

        };
        label.Transform.Position = new Vector2{ X = 400, Y = 300 };

        AddChild(rect, label);
        

        base.Start();
    }

    public override void Update()
    {
        var utils = Injector.Get<IUtils>();
        utils.MeasureTextSize((label.Path, label.Size), "i");
        if (Keyboard.KeyDown(Keys.Escape))
            Window.Running = false;

        base.Update();
    }
}