namespace GaussianElimination.Lib.Core;

public class Matrix
{
    private double[][] _matrix;

    public Matrix(int lenght1, int lenght2)
    {
        _matrix = new double[lenght1][];
        for (int i = 0; i < lenght1; i++)
        {
            _matrix[i] = new double[lenght2];
        }
    }

    public Matrix(double[][] matrix)
    {
        _matrix = matrix;
    }
    
    public Matrix(double[,] matrix)
    {
        _matrix = new double[matrix.GetLength(0)][];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            _matrix[i] = new double[matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                _matrix[i][j] = matrix[i, j];
            }
        }
    }
    
    public void SubtractRows(int minuend, int subtrahend, double scale, int startInd)
    {
        for (int i = startInd; i < _matrix.Length; i++)
        {
            _matrix[minuend][i] -= scale * _matrix[subtrahend][i];
        }
    }
    
    public int FindPivotRow(int start, int end, int column)
    {
        int maxPivotRow = start;
        double maxPivot = Math.Abs(_matrix[start][column]);
        for (int i = start+1; i < end; i++)
        {
            double pivot = Math.Abs(_matrix[i][column]);
            if (pivot > maxPivot)
            {
                maxPivotRow = i;
                maxPivot = pivot;
            }
        }

        return maxPivotRow;
    }
    
    // returns values for system with 1 as an answer for all unknowns
    public double[] GenerateValues()
    {
        Random random = new Random();
        double[] values = new double[_matrix.Length]; 
        for (int i = 0; i < _matrix.Length; i++)
        {
            for (int j = 0; j < _matrix[0].Length; j++)
            {
                _matrix[i][j] = random.Next(-10, 10);
                values[i] += _matrix[i][j];
            }
        }

        return values;
    }

    public bool IsSquare()
    {
        int size = _matrix.Length;
        for (int i = 0; i < size; i++)
        {
            if (_matrix[i].Length != size)
            {
                return false;
            }
        }

        return true;
    }

    public int Length => _matrix.Length;
    public int GetLenght(int dimension)
    {
        return dimension == 0 ? Length : _matrix[0].Length;
    }

    public double[] this[int i]
    {
        get => _matrix[i];
        set => _matrix[i] = value;
    }
    
    public double this[int i, int j]
    {
        get => _matrix[i][j];
        set => _matrix[i][j] = value;
    }

    public double[][] this[Range range]
    {
        get => _matrix[range];
    }
}