using System.Numerics;

namespace Luna;

public interface IMaterial : IDisposable
{
    Dictionary<string, float> FloatProperties { get; }
    Dictionary<string, Vector3> Vector3Properties { get;  }
    Dictionary<string, Color> ColorProperties { get; }
    Dictionary<string, Matrix4x4> MatricesProperties { get;  } 
    Dictionary<string, int> IntProperties { get; }
    Dictionary<string, bool> BoolProperties { get; }

    void SetTexture2D(string key, Texture2D texture);
    void SetCubeMap(string key, CubeMap texture);

    void Bind();
    void Unbind();
}
