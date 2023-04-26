using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class PartialPivotingAlgorithm : IAlgorithm
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
        for (int k = 0; k < n - 1; k++)
        {
            int maxPivotRow = FindPivotRow(coefficients, k);
            if (maxPivotRow != k) //swap if needed
            {
                (coefficients[k], coefficients[maxPivotRow]) = (coefficients[maxPivotRow], coefficients[k]);
                (values[k], values[maxPivotRow]) = (values[maxPivotRow], values[k]);
            }

            Parallel.For(k + 1, n, i =>
            {
                float scale = coefficients[i, k] / coefficients[k, k];
                for (int j = k + 1; j < n; j++)
                {
                    coefficients[i, j] -= scale * coefficients[k, j];
                }

                values[i] -= scale * values[k];
                coefficients[i, k] = default!;
            });
        }
    }

    private int FindPivotRow(Matrix coefficients, int start)
    {
        int maxPivotRow = start;
        float maxPivot = Math.Abs(coefficients[start, start]);
        for (int i = start+1; i < coefficients.Lenght; i++)
        {
            float pivot = Math.Abs(coefficients[i, start]);
            if (pivot > maxPivot)
            {
                maxPivotRow = i;
                maxPivot = pivot;
            }
        }

        return maxPivotRow;
    }
    
    private void BackwardElimination(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        for (int i = n - 1; i >= 0; i--)
        {
            float sum = 0;
            Parallel.For(i + 1, n, j =>
            {
                sum += coefficients[i, j] * values[j];
            });
            values[i] = (values[i] - sum) / coefficients[i, i];
        }
    }
}