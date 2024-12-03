namespace Luna.OpenGL;

public class ModelMesh(Vertex[] vertices, uint[] indices, Texture2D[]? diffuseMaps = null, Texture2D[]? specularMaps = null, Texture2D[]? normalMaps = null) : Mesh(vertices, indices)
{
    private readonly Texture2D[]? _diffuseMaps = diffuseMaps;
    private readonly Texture2D[]? _specularMaps = specularMaps;
    private readonly Texture2D[]? _normalMaps = normalMaps;

    public void BindMaterial(IStandardMaterial material)
    {
        if (_diffuseMaps is not null)
        {
            material.DiffuseMaps = _diffuseMaps;
        }

        if (_specularMaps is not null)
        {
            material.SpecularMaps = _specularMaps;
        }

        if (_normalMaps is not null)
        {
            material.NormalMaps = _normalMaps;
        }
    }
}
