using Luna.Editor;

namespace Luna.EditorPlayGround;

public class Test : Node
{
    public override void Start()
    {
        const string projectPath = "C:/Users/israe/OneDrive/Documentos/Projetos/C#/LunaTests/Test02";

        ProjectHandler.CreateProject("Test02", projectPath, 
            ["Luna.OpenGL", "Luna.Editor", "Luna.Audio", "Test02"], 
            ["Luna.Core", "Luna.OpenGL", "Luna.Audio", "Luna.Editor"], 
            ["LunaOpenGL", "LunaEditor", "LunaAudio"]);

        //var root = ProjectHandler.OpenProject(projectPath + "/Test02");

        // var camera2D = new OrtographicCamera(){
        //     Left = 0.0f,
        //     Right = Window.VirtualSize.X,
        //     Bottom = Window.VirtualSize.Y,
        //     Top = 0.0f,
        //     Listener = new()
        // };

        // var rect = new Rectangle(){
        //     Size = new(400, 400),
        //     Color = Colors.Red,
        //     Center = true,
        // };
        // rect.Transform.Position = Window.VirtualCenter / 2;

        // var light = new Light
        // {
        //     LightSource = new DirectionalLight()
        // };

        // root.AddChild(camera2D, rect, light);

        // ProjectHandler.SaveRootFile(projectPath + "/Test01", root);

        //AddChild(root);
    }

    public override void Update()
    {
        
    }
}
