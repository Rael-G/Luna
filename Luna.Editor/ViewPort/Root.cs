namespace Luna.Editor;

public class Root : Node
{
    public override void Start()
    {
        ProjectHandler.CreateProject("Text00", "../../LunaTests/Test00", [], [], []);
    }

    public override void Update()
    {
        
    }
}
