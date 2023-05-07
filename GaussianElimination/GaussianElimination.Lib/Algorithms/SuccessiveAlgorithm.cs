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
            values[0] /= coefficients[0][0];
            return values;
        }

        float[] results = new float[n];
        var taskCountdown = new CountdownEvent(2);

        Matrix coefficients2 = coefficients.Clone();
        float[] values2 = GetCopy(values, 0, n);

        ThreadPool.QueueUserWorkItem((state) =>
        {
            float[] result = LeftSplit(coefficients, values);

            lock (results)
            {
                result.CopyTo(results, n/2);
                taskCountdown.Signal();
            }
        });
        
        ThreadPool.QueueUserWorkItem((state) =>
        {
            float[] result = RightSplit(coefficients2, values2);

            lock (results)
            {
                result.CopyTo(results, 0);
                taskCountdown.Signal();
            }
        });

        taskCountdown.Wait();
        
        return results;
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