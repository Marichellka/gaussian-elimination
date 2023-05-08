using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class SuccessiveAlgorithm: IAlgorithm
{
    private int minSizeSystem;
    public SuccessiveAlgorithm(int threads, int minSizeSystem)
    {
        this.minSizeSystem = minSizeSystem;
        // ThreadPool.SetMaxThreads(threads, threads);
    }
    
    public float[] Solve(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        if (n <= minSizeSystem)
        {
            return new SequentialAlgorithm().Solve(coefficients, values);
        }

        List<float> results = new List<float>();
        var taskCountdown = new CountdownEvent(2);
        
        
        Matrix coefficients2 = coefficients.Clone();
        float[] values2 = GetCopy(values, 0, n);
        Matrix[] coeffs = new[] { coefficients, coefficients2 };
        float[][] vals = new[] { values, values2 };
        
        Parallel.Invoke(new ParallelOptions(), 
            ()=>
            {
                List<float> result = LeftSplit(coefficients, values).ToList();
                results.AddRange(result);
            },
            () =>
            {
                List<float> result = RightSplit(coefficients2, values2).ToList();
                results.AddRange(result);
            });

        // Func<Matrix, float[], float[]>[] funcs = new [] { LeftSplit, RightSplit };
        // float[][] res = new float[2][];
        // Parallel.For(0, funcs.Length, i =>
        // {
        //     res[i]=funcs[i](coeffs[i], vals[i]);
        // });
        //
        // res[0].CopyTo(results, n/2);
        // res[1].CopyTo(results, 0);

        // ThreadPool.QueueUserWorkItem((state) =>
        // {
        //     float[] result = LeftSplit(coefficients, values);
        //
        //     lock (results)
        //     {
        //         result.CopyTo(results, n/2);
        //         taskCountdown.Signal();
        //     }
        // });
        //
        // ThreadPool.QueueUserWorkItem((state) =>
        // {
        //     float[] result = RightSplit(coefficients2, values2);
        //
        //     lock (results)
        //     {
        //         result.CopyTo(results, 0);
        //         taskCountdown.Signal();
        //     }
        // });
        //
        // taskCountdown.Wait();
        
        return results.ToArray();
    }

    private float[] LeftSplit(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        ForwardElimination(coefficients, values);
        float[] result = Solve(
            coefficients.GetSubMatrix(n / 2, n / 2, n - n/2), 
            GetCopy(values, n / 2, n));
        return result;
    }
    
    private float[] RightSplit(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        BackwardElimination(coefficients, values);
        float[] result = Solve(
            coefficients.GetSubMatrix(0, 0, n / 2), 
            GetCopy(values, 0, n / 2));
        return result;
    }

    private void ForwardElimination(Matrix coefficients, float[] values)
    {
        int n = coefficients.Lenght;
        for (int k = 0; k < n / 2; k++)
        {
            int maxPivotRow = coefficients.FindPivotRow(k, n, k);
            if (maxPivotRow != k) //swap if needed
            {
                (coefficients[k], coefficients[maxPivotRow]) = (coefficients[maxPivotRow], coefficients[k]);
                (values[k], values[maxPivotRow]) = (values[maxPivotRow], values[k]);
            }
            
            for (int i = k + 1; i < n; i++)
            {
                for (int j = k + 1; j < n; j++)
                {
                    coefficients[i, j] -= coefficients[i, k] * coefficients[k, j] / coefficients[k, k];
                }
                values[i] -= coefficients[i, k] * values[k] / coefficients[k, k];
            }
        }
    }
    
    private void BackwardElimination(Matrix coefficients, float[] values)
    {
        int n = coefficients.Lenght;
        for (int k = n - 1; k >= n / 2; k--)
        {
            int maxPivotRow = coefficients.FindPivotRow( 0, k+1, k);
            if (maxPivotRow != k) //swap if needed
            {
                (coefficients[k], coefficients[maxPivotRow]) = (coefficients[maxPivotRow], coefficients[k]);
                (values[k], values[maxPivotRow]) = (values[maxPivotRow], values[k]);
            }

            for (int i = k - 1; i >= 0; i--)
            {
                for (int j = k - 1; j >= 0; j--)
                {
                    coefficients[i, j] -= coefficients[i, k] * coefficients[k, j] / coefficients[k, k];
                }
                values[i] -= coefficients[i, k] * values[k] / coefficients[k, k];
            }
        }
    }
    
    private float[] GetCopy(float[] values, int start, int end)
    {
        float[] copy = new float[end - start];
        for (int i = start; i < end; i++)
        {
            copy[i - start] = values[i];
        }

        return copy;
    }
}