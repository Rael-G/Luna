using System.Numerics;

namespace Luna.Maths;

public static class Matrix4x4Extensions
{
    public static float[] ToFloatArray(this Matrix4x4 mat)
    {
        var fMat = new float[4 * 4];
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                fMat[i * 4 + j] = mat[i, j];

        return fMat;
    }

    public static float Lenght(this Matrix4x4 mat)
    {
        var sum = 0.0;
            foreach (var data in mat.ToFloatArray())
                sum += Math.Pow(data, 2.0);
            return (float)Math.Sqrt(sum);
    }

    public static Matrix4x4 Normalize(this Matrix4x4 mat)
    {
        var aux = new Matrix4x4();
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                aux[i, j] = mat[i, j] / mat.Lenght();

        return aux;
    }

    public static Matrix4x4 Transpose(this Matrix4x4 mat)
        => Matrix4x4.Transpose(mat);

    public static Matrix4x4 Lerp(this Matrix4x4 from, Matrix4x4 to, float weight)
        => Matrix4x4.Lerp(from, to, weight);

    public static float DistanceTo(this Matrix4x4 from, Matrix4x4 to)
        => (to - from).Lenght();

    // public static Matrix4x4 FlipMatrix(Vector3 axis)
    // {
    //     var x = axis.X == 0.0f? 1.0f : -axis.X;
    //     var y = axis.Y == 0.0f? 1.0f : -axis.Y;
    //     var z = axis.Z == 0.0f? 1.0f : -axis.Z;
    //     return new 
    //     [
    //         x ,     0.0f,   0.0f, 0.0f,
    //         0.0f,   y ,     0.0f, 0.0f,
    //         0.0f,   0.0f,   z,    0.0f,
    //         0.0f,   0.0f,   0.0f, 1.0f
    //     ];
    // }

    // public static Matrix4x4 ShearMatrix(Vector3 shearFactor, Vector3 axis)
    // {
    //     axis = axis.Normalize();
    //     return new
    //         [
    //             1,                      shearFactor.Y * axis.X, shearFactor.Z * axis.X, 0,
    //             shearFactor.X * axis.Y, 1,                      shearFactor.Z * axis.Y, 0,
    //             shearFactor.X * axis.Z, shearFactor.Y * axis.Z, 1,                      0,
    //             0,                      0,                      0,                      1
    //         ];        
    // }

    public static Quaternion ToQuaternion(this Matrix4x4 mat)
    {
        float trace = mat[0, 0] + mat[1, 1] + mat[2, 2];
        float w, x, y, z;

        if (trace > 0)
        {
            float s = 0.5f / MathF.Sqrt(trace + 1.0f);
            w = 0.25f / s;
            x = (mat[2, 1] - mat[1, 2]) * s;
            y = (mat[0, 2] - mat[2, 0]) * s;
            z = (mat[1, 0] - mat[0, 1]) * s;
        }
        else
        {
            if (mat[0, 0] > mat[1, 1] && mat[0, 0] > mat[2, 2])
            {
                float s = 2.0f * MathF.Sqrt(1.0f + mat[0, 0] - mat[1, 1] - mat[2, 2]);
                w = (mat[2, 1] - mat[1, 2]) / s;
                x = 0.25f * s;
                y = (mat[0, 1] + mat[1, 0]) / s;
                z = (mat[0, 2] + mat[2, 0]) / s;
            }
            else if (mat[1, 1] > mat[2, 2])
            {
                float s = 2.0f * MathF.Sqrt(1.0f + mat[1, 1] - mat[0, 0] - mat[2, 2]);
                w = (mat[0, 2] - mat[2, 0]) / s;
                x = (mat[0, 1] + mat[1, 0]) / s;
                y = 0.25f * s;
                z = (mat[1, 2] + mat[2, 1]) / s;
            }
            else
            {
                float s = 2.0f * MathF.Sqrt(1.0f + mat[2, 2] - mat[0, 0] - mat[1, 1]);
                w = (mat[1, 0] - mat[0, 1]) / s;
                x = (mat[0, 2] + mat[2, 0]) / s;
                y = (mat[1, 2] + mat[2, 1]) / s;
                z = 0.25f * s;
            }
        }

        return new Quaternion(x, y, z, w);
    }

}
