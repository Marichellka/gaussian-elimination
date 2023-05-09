using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class RowOrientedAlgorithm: IAlgorithm
{
    private int _threadCount;
    
    public RowOrientedAlgorithm(int threadCount)
    {
        _threadCount = threadCount;
    }
    
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
            int maxPivotRow = FindGlobalPivot(coefficients, k, n, k);
            if (maxPivotRow != k)
            {
                (coefficients[k], coefficients[maxPivotRow]) = (coefficients[maxPivotRow], coefficients[k]);
                (values[k], values[maxPivotRow]) = (values[maxPivotRow], values[k]);
            }
            
            int rowsPerWorker = (n - k - 1) / _threadCount;
            int leftRows = (n - k - 1) % _threadCount;
            int offset = k + 1;
            CountdownEvent countDown = new CountdownEvent(_threadCount);
            for (int i = 0; i < _threadCount; i++)
            {
                int rows = rowsPerWorker + (i < leftRows ? 1 : 0);
                if (rows == 0)
                {
                    countDown.Signal();
                    continue;
                }
                int start = offset;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    SubtractFromRows(coefficients, values, start, start+rows, k);
                    countDown.Signal();
                });
                offset += rows;
            }

            countDown.Wait();
        }
    }

    private void SubtractFromRows(Matrix coefficients, float[] values, int start, int end, int pivot)
    {
        for (int i = start; i < end; i++)
        {
            float scale = coefficients[i, pivot] / coefficients[pivot, pivot];
            SubtractFromRow(coefficients[i], coefficients[pivot], scale, pivot+1);

            values[i] -= scale * values[pivot];
            coefficients[i, pivot] = 0;
        }
    }

    private void SubtractFromRow(float[] minuend, float[] subtrahend, float scale, int startInd)
    {
        for (int i = startInd; i < minuend.Length; i++)
        {
            minuend[i] -= scale * subtrahend[i];
        }
    }

    private int FindGlobalPivot(Matrix matrix, int start, int end, int column)
    {
        int rowsPerWorker = (end - start) / _threadCount;
        int leftRows = (end - start) % _threadCount;
        int globalPivotRow = start;
        float globalPivot = Math.Abs(matrix[start, column]);
        CountdownEvent countDown = new CountdownEvent(_threadCount);
        for (int i = 0; i < _threadCount; i++)
        {
            int rows = rowsPerWorker + (i < leftRows ? 1 : 0);
            if (rows == 0)
            {
                countDown.Signal();
                continue;
            }
            int startCopy = start;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                int localPivotRow = matrix.FindPivotRow(startCopy, startCopy+rows, column);
                if (Volatile.Read(ref globalPivot) < Math.Abs(matrix[localPivotRow, column]))
                    Interlocked.Exchange(ref globalPivotRow, localPivotRow);
                countDown.Signal();
            });
            start += rows;
        }

        countDown.Wait(); //wait for all tasks to finish
        return globalPivotRow;
    }
    
    // process back substitution on a upper triangle matrix
    private void BackwardSubstitution(Matrix coefficients, float[] values)
    {
        int n = values.Length;
        for (int i = n - 1; i >= 0; i--)
        {
            float sum = 0;
            for (int j = i+1; j < n; j++)
            {
                sum += coefficients[i, j] * values[j];
            }
            values[i] = (values[i] - sum) / coefficients[i, i];
        }
    }
}