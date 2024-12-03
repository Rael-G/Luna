using System.Numerics;
using Silk.NET.Assimp;

namespace Luna.OpenGL;

public static unsafe class ModelLoader
{
    private static readonly Assimp _assimp = Assimp.GetApi();
    
    public static List<ModelMesh> LoadMeshes(ModelData modelData)
    {
        var aiScene = _assimp.ImportFile(modelData.Path, (uint)PostProcessPreset.TargetRealTimeMaximumQuality);

        if (aiScene is null || aiScene->MRootNode is null || (aiScene->MFlags & (uint)SceneFlags.Incomplete) > 0)
            throw new ResourceException("Failed to load the model. " + _assimp.GetErrorStringS());

        var directory = Path.GetDirectoryName(modelData.Path)!;

        return ProcessNode(aiScene->MRootNode, aiScene, modelData, directory);
    }

    private static List<ModelMesh> ProcessNode(Silk.NET.Assimp.Node* aiNode, Scene* aiScene, ModelData modelData, string directory)
    {
        List<ModelMesh> meshes = [];
        for (int i = 0; i < aiNode->MNumMeshes; i++)
        {
            var aiMesh = aiScene->MMeshes[aiNode->MMeshes[i]];
            meshes.Add(ProcessMesh(aiMesh, aiScene, modelData, directory));
        }

        for (int i = 0; i < aiNode->MNumChildren; i++)
        {
            meshes.AddRange(ProcessNode(aiNode->MChildren[i], aiScene, modelData, directory));
        }

        return meshes;
    }

    private static ModelMesh ProcessMesh(Silk.NET.Assimp.Mesh* aiMesh, Scene* aiScene, ModelData modelData, string directory)
    {
        List<Vertex> vertices = [];
        List<uint> indices = [];

        for (int i = 0; i < aiMesh->MNumVertices; i++)
        {
            var vertex = new Vertex()
            {
                Position = aiMesh->MVertices[i],
                Normal = aiMesh->MNormals[i],
                Tangent = aiMesh->MTangents[i],
                Bitangent = aiMesh->MBitangents[i]
            };
            if (aiMesh->MTextureCoords[0] != null)
                vertex.TexCoords = new Vector2(aiMesh->MTextureCoords[0][i].X, aiMesh->MTextureCoords[0][i].Y);
            vertices.Add(vertex);
        }

        for (int i = 0; i < aiMesh->MNumFaces; i++)
        {
            var aiFace = aiMesh->MFaces[i];
            for (int j = 0; j < aiFace.MNumIndices; j++)
                indices.Add(aiFace.MIndices[j]);
        }

        Texture2D[]? diffuseMaps = null;
        Texture2D[]? specularMaps = null;
        Texture2D[]? normalMaps = null;

        if (aiMesh->MMaterialIndex >= 0)
        {
            var aiMaterial = aiScene->MMaterials[aiMesh->MMaterialIndex];
            diffuseMaps = ProcessTextures(aiMaterial, TextureType.Diffuse, modelData, directory);
            specularMaps = ProcessTextures(aiMaterial, TextureType.Specular, modelData, directory);
            normalMaps = ProcessTextures(aiMaterial, TextureType.Normals, modelData, directory);
            if (normalMaps.Length < 1)
            {
                normalMaps = ProcessTextures(aiMaterial, TextureType.Height, modelData, directory);
            }
        }

        return new ModelMesh([.. vertices], [.. indices], diffuseMaps, specularMaps, normalMaps);
    }

    private static Texture2D[] ProcessTextures(Silk.NET.Assimp.Material* aiMaterial, TextureType type, ModelData modelData, string directory)
    {
        List<Texture2D> textures = [];

        for (uint i = 0; i < _assimp.GetMaterialTextureCount(aiMaterial, type); i++)
        {
            AssimpString path;
            _assimp.GetMaterialTexture(aiMaterial, type, i, &path, null, null, null, null, null, null);
            textures.Add(new Texture2D()
            { 
                Path = Path.Combine(directory, path),
                FilterMode = modelData.TextureFilter,
                ImageType = GetImageType(type)
            });
        }

        return [.. textures];
    }

    private static ImageType GetImageType(TextureType textureType)
    {
        if (textureType == TextureType.Diffuse || 
        textureType == TextureType.Emissive || 
        textureType == TextureType.BaseColor || 
        textureType == TextureType.EmissionColor)
        {
            return ImageType.SRGB;
        }

        return ImageType.Linear;
    }
}
