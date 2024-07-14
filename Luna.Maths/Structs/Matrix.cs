using System.Collections;

namespace Luna.Maths;

public class Matrix : IEnumerable<double>
{
    public int Rows { get; }
    public int Columns { get; }
    public double[,] Data { get; }

    public double Length 
    { 
        get
        {
            var sum = 0.0;
            foreach (var data in Data)
                sum += Math.Pow(data, 2.0);
            return Math.Sqrt(sum);
        }
    }

    public double this[int row, int colunm]
    { 
        get => Data[row, colunm];
        set => Data[row, colunm] = value;
    }

    public Matrix(int rows, int columns)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(rows, 1, nameof(rows));
        ArgumentOutOfRangeException.ThrowIfLessThan(columns, 1, nameof(columns));
        
        Rows = rows;
        Columns = columns;
        Data = new double[rows, columns];
    }

    public Matrix(Matrix matrix)
    {
        Rows = matrix.Rows;
        Columns = matrix.Columns;
        Data = new double[Rows, Columns];
        Array.Copy(matrix.Data, Data, matrix.Data.Length);
    }

    public Matrix(double[,] data)
    {
        Rows = data.GetLength(0);
        Columns = data.GetLength(1);
        Data = new double[Rows, Columns];
        Array.Copy(data, Data, data.Length);
    }

    public static bool operator == (Matrix? a, Matrix? b)
    {
        if (a is null || b is null || a.Rows != b.Rows || a.Columns != b.Columns) 
            return false;

        for(int i = 0; i < a.Rows; i++)
            for(int j = 0; j < a.Columns; j++)
                if(a[i,j] != b[i,j]) return false;
        
        return true;
    }

    public static bool operator != (Matrix? a, Matrix? b)
        => !(a == b);
    
    public static bool operator > (Matrix? a, Matrix? b)
        => a?.Length > b?.Length;
    
    public static bool operator < (Matrix? a, Matrix? b)
        => !(a > b);

    public static bool operator >= (Matrix? a, Matrix? b)
        => a > b || a == b;
    
    public static bool operator <= (Matrix? a, Matrix? b)
        => a < b || a == b;
    
    public static Matrix operator + (Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns) 
            throw new InvalidOperationException("This operation requires matrices of the same size.");
        
        var matrix = new Matrix(a.Rows, a.Columns);

        for(int i = 0; i < a.Rows; i++)
        {
            for(int j = 0; j < a.Columns; j++)
            {
                matrix[i, j] = a[i, j] + b[i, j];
            }
        }

        return matrix;
    }

    public static Matrix operator - (Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns) 
            throw new InvalidOperationException("This operation requires matrices of the same size.");
        
        var matrix = new Matrix(a.Rows, a.Columns);

        for(int i = 0; i < a.Rows; i++)
        {
            for(int j = 0; j < a.Columns; j++)
            {
                matrix[i, j] = a[i, j] - b[i, j];
            }
        }

        return matrix;
    }

    public static Matrix operator - (Matrix a)
        => a * -1;
    

    public static Matrix operator * (Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows) 
            throw new InvalidOperationException("Matrix multiplication requires the number of columns in the first matrix to be equal to the number of rows in the second matrix.");
        
        var matrix = new Matrix(a.Rows, b.Columns);

        for(int i = 0; i <a.Rows; i++)
            for(int j = 0; j < b.Columns; j++)
            {
                var aux = 0.0;
                for (int k = 0; k < a.Columns; k++)
                     aux += a[i, k] * b[k, j];
                
                matrix[i, j] = aux;
            }
        
        return matrix;
    }

    public static Matrix operator * (Matrix a, double b)
    {
        for (int i = 0; i < a.Rows; i++)
            for (int j = 0; j < a.Columns; j++)
                a[i, j] *= b;
        return a;
    }

    public static Matrix operator * (double a, Matrix b)
        => b * a;

    public static Matrix operator / (Matrix a, double b)
    {
        for (int i = 0; i < a.Rows; i++)
            for (int j = 0; j < a.Columns; j++)
                a[i, j] /= b;
        return a;
    }
    
    public static implicit operator Matrix(double[,] data)
        => new(data);

    public static implicit operator double[,](Matrix matrix)
        => matrix.Data;
    
    public static Matrix Identity(int size)
    {
        var vector = new Matrix(size, 1);
        for (int i = 0; i < size; i++)
            vector[i, 0] = 1;
        
        return vector.Diagonal();
    }

    public Matrix Normalize()
    {
        var matrix = new Matrix(Rows, Columns);

        if (Length == 0)
            return matrix;

        for(int i = 0; i < Rows; i++)
            for(int j = 0; j < Columns; j++)
                matrix[i, j] = this[i, j] / Length;
        
        return matrix;
    }

    public Matrix Transpose()
    {
        var matrix = new Matrix(Columns, Rows);
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Columns; j++)
                matrix[j, i] = this[i, j];

        return matrix;
    }

    public Matrix Diagonal()
    {
        if (Columns > 1) throw new InvalidOperationException("This operation requires a column matrix.");
        
        var diagonal = new Matrix(Rows, Rows);

        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Rows; j++)
                diagonal[i, j] = i == j? Data[i, 0] : 0;

        return diagonal;
    }

    public Matrix Lerp(Matrix to, double weight)
    {
        if (Rows != to.Rows || Columns != to.Columns) 
            throw new InvalidOperationException("This operation requires matrices of the same size.");

        var matrix = new Matrix(Rows, Columns);
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Columns; j++)
                matrix[i,j] = Data[i, j].Lerp(to[i,j], weight);

        return matrix;
    }

    public double DistanceTo(Matrix to)
        => (to - this).Length;

    public override bool Equals(object? obj)
    {
        if (obj != null && obj is Matrix mat)
            return this == mat;

        return false;
    }

    public override int GetHashCode()
        => Data.GetHashCode();
    
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    

    public IEnumerator<double> GetEnumerator()
    {
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Columns; j++)
                yield return this[i, j];
    }
}
