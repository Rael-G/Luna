using System.Numerics;
using Luna;
using Luna.Audio;
using Luna.Core;
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

public class Root : Node
{
    Label label;
    Rectangle rect;
    Box box;
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
        var texture = new Texture2D()
        {
            Path = "Assets/images/Hamburger.png",
            TextureFilter = TextureFilter.Nearest
        };
        var camera2D = new OrtographicCamera(){
            Left = 0.0f,
            Right = Window.Size.X,
            Bottom = Window.Size.Y,
            Top = 0.0f,
            Listener = new()
        };
        var camera3D = new PerspectiveCamera()
        {
        };
        camera3D.Transform.Position = Vector3.Zero;
        var ellipse = new Ellipse
        {
            Radius = new(300, 300),
            Segments = 10,
        };

        ellipse.Transform.Position = new Vector3{ X = 400, Y = 300, Z = 0 };
        // ellipse.Material.Diffuse = texture;
        // ellipse.Material.Specular = texture;

        rect = new Rectangle(){
            Size = new(400, 400),
            Center = true,
        };
        rect.Transform.Position = Window.VirtualCenter;
        // rect.Material.Diffuse = texture;

        label = new Label("Assets/fonts/OpenSans-Regular.ttf")
        {
            Text = "Hello, World!",
            FlipV = true,
            CenterH = true,
            CenterV = true
        };
        label.Transform.Position = new Vector3{ X = 400, Y = 300, Z = 0 };
        
        var sound = new Sound("Assets/audio/music/Death.wav");
        sound.Transform.Position = Vector3.Zero;

        box = new Box()
        {
            Color = Colors.Beige,
            Center = true
        };
        box.Transform.Position = new Vector3(0f, 0f, -10f);
        box.Material.Diffuse = texture;
        box.Material.Specular = texture;
        camera3D.Target = box.Transform.Position;
        AddChild(camera3D, box);

        base.Start();
    }

    public override void Update()
    {
        // box.Transform.Rotation += Vector3.UnitX * Time.DeltaTime;
        // box.Transform.Rotation += Vector3.UnitY * Time.DeltaTime;
        // box.Transform.Rotation += Vector3.UnitZ * Time.DeltaTime;

        if (Keyboard.KeyDown(Keys.Escape))
            Window.Running = false;

        base.Update();
    }
}