using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class SequentialAlgorithm : IAlgorithm
{
    public double[] Solve(Matrix coefficients, double[] values)
    {
        ForwardElimination(coefficients, values);
        BackwardSubstitution(coefficients, values);
        return values;
    }

    private void ForwardElimination(Matrix coefficients, double[] values)
    {
        int n = values.Length;
        for (int k = 0; k < n - 1; k++)
        {
            int maxPivotRow = coefficients.FindPivotRow(k, n, k);
            if (maxPivotRow != k)
            {
                (coefficients[k], coefficients[maxPivotRow]) = (coefficients[maxPivotRow], coefficients[k]);
                (values[k], values[maxPivotRow]) = (values[maxPivotRow], values[k]);
            }

            for (int i = k + 1; i < n; i++)
            {
                double scale = coefficients[i, k] / coefficients[k, k];
                coefficients.SubtractRows(i, k, scale, k + 1);

                values[i] -= scale * values[k];
                coefficients[i, k] = 0;
            }
        }
    }

    // process back substitution on an upper triangle matrix
    private void BackwardSubstitution(Matrix coefficients, double[] values)
    {
        int n = values.Length;
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += coefficients[i, j] * values[j];
            }

            values[i] = (values[i] - sum) / coefficients[i, i];
        }
    }
}