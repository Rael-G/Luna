namespace Luna;

public class Skybox : Node
{
    public CubeMap CubeMap { get; set; }

    public override void Awake()
    {
        CreateRenderObject
        (
            new SkyboxData
            { 
                CubeMap = CubeMap, ModelViewProjection = ModelViewProjection
            }
        );
    }

    public override void LateUpdate()
    {
        UpdateRenderObject
        (
            new SkyboxData
            { 
                CubeMap = CubeMap, ModelViewProjection = ModelViewProjection
            }
        );
    }
}
