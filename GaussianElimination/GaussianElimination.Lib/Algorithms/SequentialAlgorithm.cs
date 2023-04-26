using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class SequentialAlgorithm
{
    public float[] Solve(Matrix coefficients, float[] values)
    {
        ForwardElimination(coefficients, values);
        BackwardElimination(coefficients, values);
        return values;
    }

    private void ForwardElimination(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        for (int k = 0; k < n; k++)
        {
            Normalize(coefficients, values, k);
            for (int i = k + 1; i < n; i++)
            {
                float scale = coefficients[i, k];
                for (int j = k + 1; j < n; j++)
                {
                    coefficients[i, j] -= scale * coefficients[k, j];
                }

                values[i] -= coefficients[i, k] * values[k];
                coefficients[i, k] = default!;
            }
        }
    }

    private void Normalize(Matrix coefficients, float[] values, int row)
    {
        for (int j = row + 1; j < values.Length; j++)
        {
            coefficients[row, j] /= coefficients[row, row];
        }

        values[row] /= coefficients[row, row];
        coefficients[row, row] /= coefficients[row, row];
    }

    // process back substitution on a upper triangle matrix
    private void BackwardElimination(Matrix coefficients, float[] values)
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