using System.Numerics;

namespace GaussianElimination.Lib.Core;

public class Matrix
{
    public float[][] matrix;

    public Matrix(int lenght1, int lenght2)
    {
        matrix = new float[lenght1][];
        for (int i = 0; i < lenght1; i++)
        {
            matrix[i] = new float[lenght2];
        }
    }

    public Matrix(float[][] matrix)
    {
        this.matrix = matrix;
    }
    
    public Matrix(float[,] matrix)
    {
        this.matrix = new float[matrix.GetLength(0)][];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            this.matrix[i] = new float[matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                this.matrix[i][j] = matrix[i, j];
            }
        }
    }
    
    public int FindPivotRow(int start, int end, int column)
    {
        int maxPivotRow = start;
        float maxPivot = Math.Abs(matrix[start][column]);
        for (int i = start+1; i < end; i++)
        {
            float pivot = Math.Abs(matrix[i][column]);
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
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
            {
                matrix[i][j] = random.Next(-10, 10);
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
                subMatrix[i][j] = matrix[row + i][column + j];
            }
        }

        return new Matrix(subMatrix);
    }

    public Matrix Clone()
    {
        float[][] newMatrix = new float[matrix.Length][];
        for (int i = 0; i < matrix.Length; i++)
        {
            newMatrix[i] = new float[matrix[i].Length];
            for (int j = 0; j < matrix[i].Length; j++)
            {
                newMatrix[i][j] = matrix[i][j];
            }
        }
        return new Matrix(newMatrix);
    }

    public int Lenght => matrix.Length;
    public int GetLenght(int dimension)
    {
        return dimension == 0 ? Lenght : matrix[0].Length;
    }

    public float[] this[int i]
    {
        get => matrix[i];
        set => matrix[i] = value;
    }
    
    public float this[int i, int j]
    {
        get => matrix[i][j];
        set => matrix[i][j] = value;
    }
}