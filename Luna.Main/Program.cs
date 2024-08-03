using System.Numerics;
using Luna;
using Luna.Audio;
using Luna.OpenGL;

internal class Program
{
    private static void Main(string[] args)
    {
        LunaOpenGL.AddServices();
        LunaAudio.AddServices();
        var root = new Root();
        
        
        var host = new Host(root);
        host.Run();
    }
}

public class Root : Node2D
{
    Label label;
    Rectangle rect;
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
            Top = 0.0f,
            Listener = new()
        };
        AddChild(camera);
        var ellipse = new Ellipse
        {
            Radius = new(300, 300),
            Color = Colors.Red,
            Segments = 10,
        };

        ellipse.Transform.Position = new Vector2{ X = 400, Y = 300 };

        rect = new Rectangle(){
            Size = new(400, 400),
            Color = Colors.Red,
            Center = true,
        };
        rect.Transform.Position = Window.VirtualCenter;

        label = new Label("Assets/fonts/OpenSans-Regular.ttf")
        {
            Text = "Hello, World!",
            FlipV = true,
            CenterH = true,
            CenterV = true
        };
        label.Transform.Position = new Vector2{ X = 400, Y = 300 };
        
        var sound = new Sound2D("Assets/audio/music/Death.wav");
        sound.Transform.Position = new Vector2(0, 0);
        AddChild(ellipse);

        base.Start();
    }

    public override void Update()
    {
        if (Keyboard.KeyDown(Keys.Escape))
            Window.Running = false;

        base.Update();
    }
}