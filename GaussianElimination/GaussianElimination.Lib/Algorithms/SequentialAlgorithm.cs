using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class SequentialAlgorithm<T> : IAlgorithm<T> where T :
    IMultiplyOperators<T, T, T>, ISubtractionOperators<T, T, T>,
    IDivisionOperators<T, T, T>, IAdditionOperators<T, T, T>
{
    public static T[] Solve(Matrix<T> coefficients, T[] values)
    {
        ForwardElimination(coefficients, values);
        BackwardElimination(coefficients, values);
        return values;
    }

    private static void ForwardElimination(Matrix<T> coefficients, T[] values)
    {
        int n = values.Length;
        for (int k = 0; k < n; k++)
        {
            for (int j = k + 1; j < n; j++)
            {
                coefficients[k, j] /= coefficients[k, k];
            }

            values[k] /= coefficients[k, k];
            coefficients[k, k] /= coefficients[k, k];

            for (int i = k + 1; i < n; i++)
            {
                for (int j = k + 1; j < n; j++)
                {
                    coefficients[i, j] -= coefficients[i, k] * coefficients[k, j];
                }

                values[i] -= coefficients[i, k] * values[k];
                coefficients[i, k] = default!;
            }
        }
    }

    // process back substitution on a upper triangle matrix
    private static void BackwardElimination(Matrix<T> coefficients, T[] values)
    {
        int n = values.Length;
        for (int i = n - 1; i >= 1; i--)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                values[j] -= values[i] * (coefficients[j, i] / coefficients[i, i]);
                coefficients[j, i] = default!;
            }
        }
    }
}