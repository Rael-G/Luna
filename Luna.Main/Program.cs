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
    Model model;
    Light<DirectionalLight> light;
    public override void Config()
    {
        Window.Title = "Hello Rectangle!";
        Window.Size = new(800, 600);
        Window.Flags |= WindowFlags.BackFaceCulling;
        base.Config();
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        var postProcessor = new Luna.PostProcessor()
        {
            Shaders =
            [
                new ShaderSource
                {
                    Name = "ScreenShaderVertex",
                    Path = "Assets/shaders/ScreenVertexShader.glsl",
                    ShaderType = ShaderType.VertexShader
                },
                new ShaderSource
                {
                    Name = "ScreenShaderFragment",
                    Path = "Assets/shaders/ScreenFragmentShader.glsl",
                    ShaderType = ShaderType.FragmentShader
                }
            ],
        };

        var texture = new Texture2D()
        {
            Path = "Assets/images/Hamburger.png",
            TextureFilter = TextureFilter.Nearest,
            FlipV = false
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
        ellipse.Material.DiffuseMaps = [ texture ];
        ellipse.Material.SpecularMaps = [ texture ];
        ellipse.Transform.EulerAngles += new Vector3(0, 180, 0);


        rect = new Rectangle(){
            Size = new(400, 400),
            Center = true,
            Color = Colors.Red
        };
        rect.Transform.Position = Window.VirtualCenter;
        rect.Transform.Position = new Vector3(0, 0, -500);

        rect.Transform.EulerAngles += new Vector3(0, 180, 0);
        rect.Material.DiffuseMaps = [texture];
        rect.Material.IsAffectedByLight = false;


        label = new Label("Assets/fonts/OpenSans-Regular.ttf")
        {
            Text = "Hello, World!",
            FlipV = false,
            CenterH = true,
            CenterV = true
        };
        label.Transform.Position = new Vector3{ X = 0, Y = 0, Z = -500 };
        
        var sound = new Sound("Assets/audio/music/Death.wav");
        sound.Transform.Position = Vector3.Zero;

        box = new Box()
        {
            Color = Colors.White,
            Center = true
        };
        box.Transform.Position = new Vector3(0f, 0f, -2f);
        box.Material.DiffuseMaps = [ texture ];
        box.Material.SpecularMaps = [ texture ];
        camera3D.Target = box.Transform.Position;
        
        light = new Light<DirectionalLight>();
        light.LightSource.Ambient = new Vector3(0.4f, 0.4f, 0.4f);
        light.LightSource.Specular = new Vector3(0.8f, 0.8f, 0.8f);
        light.LightSource.Diffuse = Vector3.One;
        light.Transform.EulerAngles = new Vector3(-90f, 0, 0);
        box.Material.IsAffectedByLight = false;

        model = new Model()
        {
            Path = "Assets/models/backpack/backpack.obj"
        };
        model.Transform.Position = new Vector3(0f, 0f, -5);
        
        postProcessor.AddChild(camera3D, box, light);
        AddChild(postProcessor);
        base.Start();
    }

    public override void Update()
    {
        model.Transform.Rotation += Vector3.UnitY * Time.DeltaTime;
        box.Transform.Rotation += Vector3.UnitX * Time.DeltaTime;
        label.Transform.Rotation += Vector3.UnitY * Time.DeltaTime;

        if (Keyboard.KeyDown(Keys.Escape))
            Window.Running = false;

        base.Update();
    }
}