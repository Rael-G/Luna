// using System.Collections;
// using System.Numerics;

// namespace Luna.Maths;

// public class Matrix : IEnumerable<float>
// {
//     public int Rows { get; }
//     public int Columns { get; }
//     public float[] Data { get; }

//     public float Length
//     {
//         get
//         {
//             var sum = 0.0;
//             foreach (var data in Data)
//                 sum += Math.Pow(data, 2.0);
//             return (float)Math.Sqrt(sum);
//         }
//     }

//     public float this[int row, int column]
//     {
//         get => Data[row * Columns + column];
//         set => Data[row * Columns + column] = value;
//     }

//     public Matrix(int rows, int columns)
//     {
//         ArgumentOutOfRangeException.ThrowIfLessThan(rows, 1, nameof(rows));
//         ArgumentOutOfRangeException.ThrowIfLessThan(columns, 1, nameof(columns));

//         Rows = rows;
//         Columns = columns;
//         Data = new float[rows * columns];
//     }

//     public Matrix(int rows, int columns, float[] data)
//     {
//         Rows = rows;
//         Columns = columns;
//         Data = data;
//     }

//     public Matrix(float[,] data)
//     {
//         Rows = data.GetLength(0);
//         Columns = data.GetLength(1);
//         Data = new float[Rows * Columns];
//         for (int i = 0; i < Rows; i++)
//             for (int j = 0; j < Columns; j++)
//                 this[i, j] = data[i, j];
//     }

//     public static bool operator ==(Matrix? a, Matrix? b)
//     {
//         if (a is null || b is null || a.Rows != b.Rows || a.Columns != b.Columns)
//             return false;

//         for (int i = 0; i < a.Rows; i++)
//             for (int j = 0; j < a.Columns; j++)
//                 if (a[i, j] != b[i, j]) return false;

//         return true;
//     }

//     public static bool operator !=(Matrix? a, Matrix? b)
//         => !(a == b);

//     public static bool operator >(Matrix? a, Matrix? b)
//         => a?.Length > b?.Length;

//     public static bool operator <(Matrix? a, Matrix? b)
//         => !(a > b);

//     public static bool operator >=(Matrix? a, Matrix? b)
//         => a > b || a == b;

//     public static bool operator <=(Matrix? a, Matrix? b)
//         => a < b || a == b;

//     public static Matrix operator +(Matrix a, Matrix b)
//     {
//         if (a.Rows != b.Rows || a.Columns != b.Columns)
//             throw new InvalidOperationException("This operation requires matrices of the same size.");

//         var matrix = new Matrix(a.Rows, a.Columns);

//         for (int i = 0; i < a.Rows; i++)
//         {
//             for (int j = 0; j < a.Columns; j++)
//             {
//                 matrix[i, j] = a[i, j] + b[i, j];
//             }
//         }

//         return matrix;
//     }

//     public static Matrix operator -(Matrix a, Matrix b)
//     {
//         if (a.Rows != b.Rows || a.Columns != b.Columns)
//             throw new InvalidOperationException("This operation requires matrices of the same size.");

//         var matrix = new Matrix(a.Rows, a.Columns);

//         for (int i = 0; i < a.Rows; i++)
//         {
//             for (int j = 0; j < a.Columns; j++)
//             {
//                 matrix[i, j] = a[i, j] - b[i, j];
//             }
//         }

//         return matrix;
//     }

//     public static Matrix operator -(Matrix a)
//         => a * -1;

//     public static Matrix operator *(Matrix a, Matrix b)
//     {
//         if (a.Columns != b.Rows)
//             throw new InvalidOperationException($"Matrix multiplication requires the number of columns in the first matrix to be equal to the number of rows in the second matrix. Left.Columns = {a.Columns}, Right.Rows = {b.Rows}");

//         var matrix = new Matrix(a.Rows, b.Columns);

//         for (int i = 0; i < a.Rows; i++)
//             for (int j = 0; j < b.Columns; j++)
//             {
//                 var aux = 0.0f;
//                 for (int k = 0; k < a.Columns; k++)
//                     aux += a[i, k] * b[k, j];

//                 matrix[i, j] = aux;
//             }

//         return matrix;
//     }

//     public static Matrix operator *(Matrix a, float b)
//     {
//         for (int i = 0; i < a.Rows; i++)
//             for (int j = 0; j < a.Columns; j++)
//                 a[i, j] *= b;
//         return a;
//     }

//     public static Matrix operator *(float a, Matrix b)
//         => b * a;

//     public static Matrix operator /(Matrix a, float b)
//     {
//         for (int i = 0; i < a.Rows; i++)
//             for (int j = 0; j < a.Columns; j++)
//                 a[i, j] /= b;
//         return a;
//     }

//     public static implicit operator Matrix(float[,] data)
//         => new(data);

//     public static implicit operator Matrix(Matrix4x4 data)
//     {
//         var matrix = new Matrix(4, 4);
//         matrix[0, 0] = data.M11;
//         matrix[0, 1] = data.M12;
//         matrix[0, 2] = data.M13;
//         matrix[0, 3] = data.M14;
//         matrix[1, 0] = data.M21;
//         matrix[1, 1] = data.M22;
//         matrix[1, 2] = data.M23;
//         matrix[1, 3] = data.M24;
//         matrix[2, 0] = data.M31;
//         matrix[2, 1] = data.M32;
//         matrix[2, 2] = data.M33;
//         matrix[2, 3] = data.M34;
//         matrix[3, 0] = data.M41;
//         matrix[3, 1] = data.M42;
//         matrix[3, 2] = data.M43;
//         matrix[3, 3] = data.M44;

        
//         return matrix;
//     }

//     public static implicit operator Matrix4x4(Matrix data)
//     {
//         if (data.Rows != 4 || data.Columns != 4)
//         {
//             throw new InvalidOperationException("Matrix must be 4x4 to convert to Matrix4x4.");
//         }

//         return new Matrix4x4(
//             data[0, 0], data[0, 1], data[0, 2], data[0, 3],
//             data[1, 0], data[1, 1], data[1, 2], data[1, 3],
//             data[2, 0], data[2, 1], data[2, 2], data[2, 3],
//             data[3, 0], data[3, 1], data[3, 2], data[3, 3]
//         );
//     }

//     public static implicit operator float[,](Matrix matrix)
//     {
//         var data = new float[matrix.Rows, matrix.Columns];
//         for (int i = 0; i < matrix.Rows; i++)
//             for (int j = 0; j < matrix.Columns; j++)
//                 data[i, j] = matrix[i, j];
//         return data;
//     }

//     public static implicit operator float[](Matrix matrix)
//         => matrix.Data;
    
//     public static Matrix Identity(int size)
//     {
//         var vector = new Matrix(size, 1);
//         for (int i = 0; i < size; i++)
//             vector[i, 0] = 1;

//         return vector.Diagonal();
//     }

//     public Matrix Normalize()
//     {
//         var matrix = new Matrix(Rows, Columns);

//         if (Length == 0)
//             return matrix;

//         for (int i = 0; i < Rows; i++)
//             for (int j = 0; j < Columns; j++)
//                 matrix[i, j] = this[i, j] / Length;

//         return matrix;
//     }

//     public Matrix Transpose()
//     {
//         var matrix = new Matrix(Columns, Rows);
//         for (int i = 0; i < Rows; i++)
//             for (int j = 0; j < Columns; j++)
//                 matrix[j, i] = this[i, j];

//         return matrix;
//     }

//     public Matrix Diagonal()
//     {
//         if (Columns > 1) ArgumentOutOfRangeException.ThrowIfGreaterThan(Columns, 1, nameof(Columns));

//         var diagonal = new Matrix(Rows, Rows);

//         for (int i = 0; i < Rows; i++)
//             for (int j = 0; j < Rows; j++)
//                 diagonal[i, j] = i == j ? this[i, 0] : 0;

//         return diagonal;
//     }

//     public Matrix Lerp(Matrix to, float weight)
//     {
//         if (Rows != to.Rows || Columns != to.Columns)
//             throw new InvalidOperationException("This operation requires matrices of the same size.");

//         var matrix = new Matrix(Rows, Columns);
//         for (int i = 0; i < Rows; i++)
//             for (int j = 0; j < Columns; j++)
//                 matrix[i, j] = this[i, j].Lerp(to[i, j], weight);

//         return matrix;
//     }

//     public Quaternion ToQuaternion()
//     {
//         if (Rows < 3 || Columns < 3)
//             throw new InvalidOperationException("ToQuaternion requires a 3x3 or 4x4 matrix.");

//         float trace = this[0, 0] + this[1, 1] + this[2, 2];
//         float w, x, y, z;

//         if (trace > 0)
//         {
//             float s = 0.5f / MathF.Sqrt(trace + 1.0f);
//             w = 0.25f / s;
//             x = (this[2, 1] - this[1, 2]) * s;
//             y = (this[0, 2] - this[2, 0]) * s;
//             z = (this[1, 0] - this[0, 1]) * s;
//         }
//         else
//         {
//             if (this[0, 0] > this[1, 1] && this[0, 0] > this[2, 2])
//             {
//                 float s = 2.0f * MathF.Sqrt(1.0f + this[0, 0] - this[1, 1] - this[2, 2]);
//                 w = (this[2, 1] - this[1, 2]) / s;
//                 x = 0.25f * s;
//                 y = (this[0, 1] + this[1, 0]) / s;
//                 z = (this[0, 2] + this[2, 0]) / s;
//             }
//             else if (this[1, 1] > this[2, 2])
//             {
//                 float s = 2.0f * MathF.Sqrt(1.0f + this[1, 1] - this[0, 0] - this[2, 2]);
//                 w = (this[0, 2] - this[2, 0]) / s;
//                 x = (this[0, 1] + this[1, 0]) / s;
//                 y = 0.25f * s;
//                 z = (this[1, 2] + this[2, 1]) / s;
//             }
//             else
//             {
//                 float s = 2.0f * MathF.Sqrt(1.0f + this[2, 2] - this[0, 0] - this[1, 1]);
//                 w = (this[1, 0] - this[0, 1]) / s;
//                 x = (this[0, 2] + this[2, 0]) / s;
//                 y = (this[1, 2] + this[2, 1]) / s;
//                 z = 0.25f * s;
//             }
//         }

//         return new Quaternion(x, y, z, w);
//     }

//     public float DistanceTo(Matrix to)
//         => (to - this).Length;

//     public override bool Equals(object? obj)
//     {
//         if (obj != null && obj is Matrix mat)
//             return this == mat;

//         return false;
//     }

//     public override int GetHashCode()
//         => Data.GetHashCode();

//     IEnumerator IEnumerable.GetEnumerator()
//         => GetEnumerator();

//     public IEnumerator<float> GetEnumerator()
//     {
//         for (int i = 0; i < Rows; i++)
//             for (int j = 0; j < Columns; j++)
//                 yield return this[i, j];
//     }
// }
