namespace Luna.OpenGL;

public class ModelMesh(Vertex[] vertices, uint[] indices, Texture2D[]? diffuseMaps = null, Texture2D[]? specularMaps = null) : Mesh(vertices, indices)
{
    private readonly Texture2D[]? _diffuseMaps = diffuseMaps;
    private readonly Texture2D[]? _specularMaps = specularMaps;

    public void BindMaterial(IStandardMaterial material)
    {
        if (_diffuseMaps is not null)
            material.DiffuseMaps = _diffuseMaps;

        if (_specularMaps is not null)
            material.SpecularMaps = _specularMaps;
    }
}
