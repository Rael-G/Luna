using System.Numerics;

namespace Luna.OpenGL;

public static class Tangent
{
    public static Vertex[] CalculateTangents(Vertex[] vertices, uint[] indices)
    {
        if (indices.Length % 3 != 0)
            throw new ArgumentException("Indices array must have a multiple of 3 elements.");

        Vector3[] tangents = new Vector3[vertices.Length];
        Vector3[] bitangents = new Vector3[vertices.Length];

        for (int i = 0; i < indices.Length; i += 3)
        {
            uint i0 = indices[i];
            uint i1 = indices[i + 1];
            uint i2 = indices[i + 2];

            Vertex v0 = vertices[i0];
            Vertex v1 = vertices[i1];
            Vertex v2 = vertices[i2];

            Vector3 edge1 = v1.Position - v0.Position;
            Vector3 edge2 = v2.Position - v0.Position;

            Vector2 deltaUV1 = v1.TexCoords - v0.TexCoords;
            Vector2 deltaUV2 = v2.TexCoords - v0.TexCoords;

            float f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);

            Vector3 tangent = f * (deltaUV2.Y * edge1 - deltaUV1.Y * edge2);

            Vector3 bitangent = f * (-deltaUV2.X * edge1 + deltaUV1.X * edge2);

            tangents[i0] += tangent;
            tangents[i1] += tangent;
            tangents[i2] += tangent;

            bitangents[i0] += bitangent;
            bitangents[i1] += bitangent;
            bitangents[i2] += bitangent;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].Tangent = Vector3.Normalize(tangents[i]);
            vertices[i].Bitangent = Vector3.Normalize(bitangents[i]);
        }

        return vertices;
    }
}
