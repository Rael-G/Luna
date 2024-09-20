using System.Numerics;
using Luna;
using Luna.Audio;
using Luna.Core;
using Luna.Core.Events;
using Luna.Maths;
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
    Light light;
    PerspectiveCamera camera3D;
    Luna.PostProcessor postProcessor;

    readonly Vector2[] resolutions =
    [
        new Vector2(1280, 720),  // HD
        new Vector2(1366, 768),  // HD+ (16:9 aproximado)
        new Vector2(1600, 900),  // HD+ (quase Full HD)
        new Vector2(1920, 1080), // Full HD
        new Vector2(2560, 1440), // Quad HD
        //new Vector2(3840, 2160)  // 4K UHD
    ];

    int resolutionsIndex = 4;


    public override void Config()
    {
        Window.Title = "Hello Rectangle!";
        Window.Size = resolutions[resolutionsIndex];
        Window.Flags |= WindowFlags.BackFaceCulling;
        Window.MSAA = 2;
        base.Config();
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        postProcessor = new Luna.PostProcessor()
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
            MSAA = true
        };

        var texture = new Texture2D()
        {
            Path = "Assets/images/Hamburger.png",
            TextureFilter = TextureFilter.Nearest,
            FlipV = false
        };
        var camera2D = new OrtographicCamera(){
            Left = 0.0f,
            Right = Window.VirtualSize.X,
            Bottom = Window.VirtualSize.Y,
            Top = 0.0f,
            Listener = new()
        };
        camera3D = new PerspectiveCamera()
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
        ellipse.Material.IsAffectedByLight = false;


        rect = new Rectangle(){
            Size = new(400, 400),
            Center = false,
        };
        rect.Transform.Position = Window.VirtualCenter;
        rect.Transform.Position = new Vector3(0, 0, 0);
        rect.Transform.Position = Vector3.Zero;

        rect.Material.DiffuseMaps = [texture];
        rect.Material.IsAffectedByLight = false;


        label = new Label("Assets/fonts/OpenSans-Regular.ttf")
        {
            Text = "Hello, World!",
            FlipV = true,
            CenterH = true,
            CenterV = true
        };
        label.Transform.Position = Window.VirtualCenter;
        label.Transform.Position = Vector3.Zero;
        
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
        
        light = new Light(new DirectionalLight());
        // light.LightSource.Ambient = new Vector3(0.4f, 0.4f, 0.4f);
        // light.LightSource.Specular = new Vector3(0.8f, 0.8f, 0.8f);
        // light.LightSource.Diffuse = Vector3.One;
        light.Transform.EulerAngles = new Vector3(-90f, 0, 0);
        box.Material.IsAffectedByLight = false;

        model = new Model()
        {
            Path = "Assets/models/backpack/backpack.obj"
        };
        model.Transform.Position = new Vector3(0f, 0f, -5);

        var skybox = new Skybox()
        {
            CubeMap = new()
            {
                Paths = [
                    "Assets/images/skybox/right.jpg",
                    "Assets/images/skybox/left.jpg",
                    "Assets/images/skybox/top.jpg",
                    "Assets/images/skybox/bottom.jpg",
                    "Assets/images/skybox/front.jpg",
                    "Assets/images/skybox/back.jpg",
                ]
            }
        };
        
        postProcessor.AddChild(camera3D, model, light);
        //postProcessor.UpdateAction = () => postProcessor.Resolution = Window.Size;
        //AddChild(skybox ,camera3D, model, ellipse, rect, label, light);
        
        //AddChild(camera3D, model, light);
        AddChild(postProcessor);
        base.Start();
    }

    public override void Update()
    {
        //model.Transform.Rotation += Vector3.UnitY * Time.DeltaTime;
        // box.Transform.Rotation += Vector3.UnitX * Time.DeltaTime;
        //label.Transform.Rotation += Vector3.UnitY * Time.DeltaTime;

        if (Keyboard.KeyDown(Keys.Escape))
            Window.Running = false;

        light.LightSource.Direction = camera3D.Target - camera3D.Transform.Position;

        Movement();
        camera3D.Target = camera3D.Transform.GlobalPosition + cameraFront.Normalize();
        
        base.Update();
    }

    public override void Input(InputEvent inputEvent)
    {
        if (inputEvent is MousePositionEvent mouseEvent)
            Mouse((float)mouseEvent.X, (float)mouseEvent.Y);

        if (inputEvent is MouseScrollEvent scrollEvent)
            Scroll((float)scrollEvent.Y);

        if (inputEvent is MouseButtonEvent buttonEvent)
        {
            if (buttonEvent.Button == MouseButton.Left && buttonEvent.Action == InputAction.Down)
                Window.CursorMode = CursorMode.Disabled;
        }

        if (inputEvent is KeyboardEvent keyEvent)
        {
            if (keyEvent.Key == Keys.Tab && keyEvent.Action == InputAction.Down)
                Window.CursorMode = CursorMode.Normal;

            if (keyEvent.Key == Keys.PageUp && keyEvent.Action == InputAction.Down)
            {
                resolutionsIndex = (resolutionsIndex + 1) % resolutions.Length;
                postProcessor.Resolution = resolutions[resolutionsIndex];
            }

            if (keyEvent.Key == Keys.PageDown && keyEvent.Action == InputAction.Down)
            {
                resolutionsIndex = (resolutionsIndex - 1 + resolutions.Length) % resolutions.Length;
                postProcessor.Resolution = resolutions[resolutionsIndex];
            }
        }
    }

    bool firstMouse = true;
    float yaw   = -90.0f;	// yaw is initialized to -90.0 degrees since a yaw of 0.0 results in a direction vector pointing to the right so we initially rotate a bit to the left.
    float pitch =  0.0f;
    float lastX =  Window.Size.X / 2;
    float lastY =  Window.Size.Y / 2;
    float fov   =  65.0f;
    Vector3 cameraFront;

    public void Movement()
    {
        float cameraSpeed;
        var right = cameraFront.Cross(camera3D.Up).Normalize();

        if (Keyboard.KeyDown(Keys.ShiftLeft))
            cameraSpeed = 10f * Time.DeltaTime;
        else
            cameraSpeed = 5f * Time.DeltaTime;

        if (Keyboard.KeyDown(Keys.W))
            camera3D.Transform.Position += cameraSpeed * cameraFront;
        if (Keyboard.KeyDown(Keys.S))
            camera3D.Transform.Position += -cameraSpeed * cameraFront;
        if (Keyboard.KeyDown(Keys.A))
            camera3D.Transform.Position += -cameraSpeed * right;
        if (Keyboard.KeyDown(Keys.D))
            camera3D.Transform.Position += cameraSpeed * right;
        if (Keyboard.KeyDown(Keys.Space))
            camera3D.Transform.Position += cameraSpeed * camera3D.Up;
        if (Keyboard.KeyDown(Keys.ControlLeft))
            camera3D.Transform.Position += -cameraSpeed * camera3D.Up;
    }

    public void Mouse(float xpos, float ypos)
    {

        if (firstMouse)
        {
            lastX = xpos;
            lastY = ypos;
            firstMouse = false;
        }

        float xoffset = xpos - lastX;
        float yoffset = lastY - ypos;
        lastX = xpos;
        lastY = ypos;

        float sensitivity = 0.1f; 
        xoffset *= sensitivity;
        yoffset *= sensitivity;

        yaw += xoffset;
        pitch += yoffset;

        if (pitch > 89.0f)
            pitch = 89.0f;
        if (pitch < -89.0f)
            pitch = -89.0f;

        Vector3 front;
        front.X = (float)Math.Cos(yaw.ToRadians()) * (float)Math.Cos(pitch.ToRadians());
        front.Y = (float)Math.Sin(pitch.ToRadians());
        front.Z = (float)Math.Sin(yaw.ToRadians()) * (float)Math.Cos(pitch.ToRadians());
    
        cameraFront = Vector3.Normalize(front);
    }

    public void Scroll(float yoffset)
    {
        fov -= yoffset;
        if (fov < 1.0f)
            fov = 1.0f;
        if (fov > 45.0f)
            fov = 45.0f;

        camera3D.Fov = fov.ToRadians();
    }

}
