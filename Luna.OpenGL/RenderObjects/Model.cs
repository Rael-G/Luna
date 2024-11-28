﻿namespace Luna.OpenGL;

internal class Model(ModelData data) : RenderObject<ModelData>
{
    private readonly List<ModelMesh> _meshes = ModelLoader.LoadMeshes(data);
    private ModelData _modelData = data;

    public override void Draw()
    {
        foreach (var mesh in _meshes)
        {
            mesh.BindMaterial(_modelData.Material);
            _modelData.Material.Bind();
            mesh.Draw(Silk.NET.OpenGL.PrimitiveType.Triangles);
        }
    }

    public override void Draw(IMaterial material)
    {
        material.MatricesProperties["model"] = _modelData.Material.ModelViewProjection.Model;
        material.Bind();
        foreach (var mesh in _meshes)
        {
            mesh.Draw(Silk.NET.OpenGL.PrimitiveType.Triangles);
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
