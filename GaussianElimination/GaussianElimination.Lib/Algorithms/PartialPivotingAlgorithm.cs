﻿using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class PartialPivotingAlgorithm : IAlgorithm
{
    public float[] Solve(Matrix coefficients, float[] values)
    {
        ForwardElimination(coefficients, values);
        BackwardSubstitution(coefficients, values);
        return values;
    }
    
    private void ForwardElimination(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        for (int k = 0; k < n - 1; k++)
        {
            int maxPivotRow = coefficients.FindPivotRow(k, n, k);
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

    private void BackwardSubstitution(Matrix coefficients, float[] values)
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