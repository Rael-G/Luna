using Luna.Maths;

namespace Luna.OpenGl;

internal static class MatrixExtensions
{
    public static float[] AsSpan(this Matrix matrix)
    {
        matrix = matrix.Transpose();
        var result = new float[matrix.Rows * matrix.Columns];
        var counter = 0;
        for (int i = 0; i < matrix.Rows; i++)
        {
            for (int j = 0; j < matrix.Columns; j++)
            {
                result[counter] = (float)matrix[i, j];
                counter++;
            }
        }

        return result;
    }
}
