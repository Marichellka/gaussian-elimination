using System.Numerics;

namespace GaussianElimination.Lib.Core;

public class Matrix
{
    private float[][] matrix;

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
    
    public void GenerateValues(Func<float> generator)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i][j] = generator();
            }
        }
    }

    public int Lenght => GetLenght(0);
    public int GetLenght(int dimension)
    {
        return matrix.GetLength(dimension);
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