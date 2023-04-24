using System.Numerics;

namespace GaussianElimination.Lib.Core;

public class Matrix<T> where T : 
    IMultiplyOperators<T, T, T>, ISubtractionOperators<T, T, T>,
    IDivisionOperators<T, T, T>, IAdditionOperators<T, T, T>
{
    private T[][] matrix;

    public Matrix(int lenght1, int lenght2)
    {
        matrix = new T[lenght1][];
        for (int i = 0; i < lenght1; i++)
        {
            matrix[i] = new T[lenght2];
        }
    }

    public Matrix(T[][] matrix)
    {
        this.matrix = matrix;
    }
    
    public Matrix(T[,] matrix)
    {
        this.matrix = new T[matrix.GetLength(0)][];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            this.matrix[i] = new T[matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                this.matrix[i][j] = matrix[i, j];
            }
        }
    }
    
    public void GenerateValues(Func<T> generator)
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

    public T[] this[int i]
    {
        get => matrix[i];
        set => matrix[i] = value;
    }
    
    public T this[int i, int j]
    {
        get => matrix[i][j];
        set => matrix[i][j] = value;
    }
}