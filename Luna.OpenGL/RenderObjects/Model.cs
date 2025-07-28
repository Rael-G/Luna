using Silk.NET.OpenGL;

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
        SetMVP(material, _modelData.ModelViewProjection);
        foreach (var mesh in _meshes)
        {
            material.Bind();
            if (material is IStandardMaterial standardMaterial)
            {
                mesh.BindMaterial(standardMaterial);
            }
            mesh.Draw(PrimitiveType.Triangles);
            material.Unbind();
        }
    }

    public override void Update(ModelData data)
    {
        _modelData = data;
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        foreach (var mesh in _meshes)
            mesh.Dispose();

        base.Dispose(disposing);
    }
}
