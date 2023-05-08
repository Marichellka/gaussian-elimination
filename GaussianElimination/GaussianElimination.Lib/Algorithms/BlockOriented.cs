using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class BlockOriented: IAlgorithm
{
    public float[] Solve(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        const int blockSize = 10;
        var x = new float[n];

        // Initialize A and b
        // for (int i = 0; i < n; i++)
        // {
        //     for (int j = 0; j < n; j++)
        //     {
        //         coefficients[i, j] = i == j ? 2 : 1;
        //     }
        //     values[i] = n + 1;
        // }

        // Perform Gaussian elimination in parallel blocks
        for (int k = 0; k < n; k += blockSize)
        {
            // Perform elimination in the current block
            Parallel.For(k, Math.Min(k + blockSize, n), i =>
            {
                for (int j = k + 1; j < n; j++)
                {
                    var factor = coefficients[j, k] / coefficients[k, k];
                    for (int l = k + 1; l < n; l++)
                    {
                        coefficients[j, l] -= factor * coefficients[k, l];
                    }
                    values[j] -= factor * values[k];
                }
            });

        }

        // Perform backward substitution
        for (int i = n - 1; i >= 0; i--)
        {
            for (int j = i + 1; j < n; j++)
            {
                values[i] -= coefficients[i, j] * values[j];
            }
            values[i] /= coefficients[i, i];
        }

        return values;
    }
}