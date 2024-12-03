namespace Luna.OpenGL;

internal class Model(ModelData data) : RenderObject<ModelData>
{
    private readonly List<ModelMesh> _meshes = ModelLoader.LoadMeshes(data);
    private ModelData _modelData = data;

    public override void Draw()
    {
        Draw(_modelData.Material);
    }

    public override void Draw(IMaterial material)
    {
        material.MatricesProperties["model"] = _modelData.Material.ModelViewProjection.Model;
        foreach (var mesh in _meshes)
        {
            material.Bind();
            if (material is IStandardMaterial standardMaterial)
            {
                mesh.BindMaterial(standardMaterial);
            }
            mesh.Draw(Silk.NET.OpenGL.PrimitiveType.Triangles);
            material.Unbind();
        }
    }

    public override void Update(ModelData data)
    {
        
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        foreach (var mesh in _meshes)
            mesh.Dispose();

        base.Dispose(disposing);
    }
}
