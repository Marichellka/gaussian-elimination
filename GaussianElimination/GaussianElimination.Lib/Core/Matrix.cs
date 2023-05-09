using System.Numerics;

namespace GaussianElimination.Lib.Core;

public class Matrix
{
    private float[][] _matrix;

    public Matrix(int lenght1, int lenght2)
    {
        _matrix = new float[lenght1][];
        for (int i = 0; i < lenght1; i++)
        {
            _matrix[i] = new float[lenght2];
        }
    }

    public Matrix(float[][] matrix)
    {
        this._matrix = matrix;
    }
    
    public Matrix(float[,] matrix)
    {
        this._matrix = new float[matrix.GetLength(0)][];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            this._matrix[i] = new float[matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                this._matrix[i][j] = matrix[i, j];
            }
        }
    }
    
    public void SubtractFromRow(int minuend, int subtrahend, float scale, int startInd)
    {
        for (int i = startInd; i < _matrix.Length; i++)
        {
            _matrix[minuend][i] -= scale * _matrix[subtrahend][i];
        }
    }
    
    public int FindPivotRow(int start, int end, int column)
    {
        int maxPivotRow = start;
        float maxPivot = Math.Abs(_matrix[start][column]);
        for (int i = start+1; i < end; i++)
        {
            float pivot = Math.Abs(_matrix[i][column]);
            if (pivot > maxPivot)
            {
                maxPivotRow = i;
                maxPivot = pivot;
            }
        }

        return maxPivotRow;
    }
    
    public void GenerateValues()
    {
        Random random = new Random();
        for (int i = 0; i < _matrix.Length; i++)
        {
            for (int j = 0; j < _matrix[0].Length; j++)
            {
                _matrix[i][j] = random.Next(-10, 10);
            }
        }
    }

    public Matrix GetSubMatrix(int row, int column, int size)
    {
        float[][] subMatrix = new float[size][];
        for (int i = 0; i < size; i++)
        {
            subMatrix[i] = new float[size];
            for (int j = 0; j < size; j++)
            {
                subMatrix[i][j] = _matrix[row + i][column + j];
            }
        }

        return new Matrix(subMatrix);
    }

    public Matrix Clone()
    {
        float[][] newMatrix = new float[_matrix.Length][];
        for (int i = 0; i < _matrix.Length; i++)
        {
            newMatrix[i] = new float[_matrix[i].Length];
            for (int j = 0; j < _matrix[i].Length; j++)
            {
                newMatrix[i][j] = _matrix[i][j];
            }
        }
        return new Matrix(newMatrix);
    }

    public int Lenght => _matrix.Length;
    public int GetLenght(int dimension)
    {
        return dimension == 0 ? Lenght : _matrix[0].Length;
    }

    public float[] this[int i]
    {
        get => _matrix[i];
        set => _matrix[i] = value;
    }
    
    public float this[int i, int j]
    {
        get => _matrix[i][j];
        set => _matrix[i][j] = value;
    }
}