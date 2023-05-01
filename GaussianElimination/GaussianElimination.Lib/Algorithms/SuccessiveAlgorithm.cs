using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class SuccessiveAlgorithm: IAlgorithm
{
    public float[] Solve(Matrix coefficients, float[] values)
    {
        int n = coefficients.Lenght;
        if (n == 1)
        {
            values[0] /= coefficients[0][0];
            return values;
        }
        
        Matrix coefficients2 = coefficients.Clone();
        float[] values2 = GetCopy(values, 0, n);
        
        ForwardElimination(coefficients, values);
        float[] result1 = Solve(
            coefficients.GetSubMatrix(n / 2, n / 2, n - n/2), 
            GetCopy(values, n / 2, n));
        
        BackwardElimination(coefficients2, values2);
        float[] result2 = Solve(
            coefficients2.GetSubMatrix(0, 0, n / 2), 
            GetCopy(values2, 0, n / 2));
        
        result1.CopyTo(values, 0);
        result2.CopyTo(values, n/2);
        return values;
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

    private void ForwardElimination(Matrix coefficients, float[] values)
    {
        int n = coefficients.Lenght;
        for (int k = 0; k < n / 2; k++)
        {
            int maxPivotRow = FindPivotRow(coefficients, k, n, k);
            if (maxPivotRow != k) //swap if needed
            {
                (coefficients[k], coefficients[maxPivotRow]) = (coefficients[maxPivotRow], coefficients[k]);
                (values[k], values[maxPivotRow]) = (values[maxPivotRow], values[k]);
            }

            for (int i = k + 1; i < n; i++)
            {
                coefficients[k, i] /= coefficients[k, k];
            }
            values[k] /= coefficients[k, k];

            for (int i = k + 1; i < n; i++)
            {
                for (int j = k + 1; j < n; j++)
                {
                    coefficients[i, j] -= coefficients[i, k] * coefficients[k, j];
                }
                values[i] -= coefficients[i, k] * values[k];
            }
        }
    }
    
    private int FindPivotRow(Matrix coefficients, int start, int end, int column)
    {
        int maxPivotRow = start;
        float maxPivot = Math.Abs(coefficients[start, column]);
        for (int i = start+1; i < end; i++)
        {
            float pivot = Math.Abs(coefficients[i, column]);
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
        int n = coefficients.Lenght;
        for (int k = n - 1; k >= n / 2; k--)
        {
            int maxPivotRow = FindPivotRow(coefficients, 0, k+1, k);
            if (maxPivotRow != k) //swap if needed
            {
                (coefficients[k], coefficients[maxPivotRow]) = (coefficients[maxPivotRow], coefficients[k]);
                (values[k], values[maxPivotRow]) = (values[maxPivotRow], values[k]);
            }

            for (int i = k - 1; i >= 0; i--)
            {
                coefficients[k, i] /= coefficients[k, k];
            }
            values[k] /= coefficients[k, k];

            for (int i = k - 1; i >= 0; i--)
            {
                for (int j = k - 1; j >= 0; j--)
                {
                    coefficients[i, j] -= coefficients[i, k] * coefficients[k, j];
                }
                values[i] -= coefficients[i, k] * values[k];
            }
        }
    }
}