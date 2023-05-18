using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public class RowOrientedAlgorithm: Algorithm
{
    private int _threadCount;
    private object _locker = new ();
    
    public RowOrientedAlgorithm(int threadCount)
    {
        _threadCount = threadCount;
        ThreadPool.SetMaxThreads(threadCount, threadCount);
    }
    
    public override double[] Solve(Matrix coefficients, double[] values)
    {
        ValidateSystem(coefficients, values);
        ForwardElimination(coefficients, values);
        BackwardSubstitution(coefficients, values);
        return values;
    }
    
    private void ForwardElimination(Matrix coefficients, double[] values)
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
                    countDown.Signal(_threadCount-i);
                    break;
                }
                int start = offset;
                int pivot = k;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    SubtractFromRows(
                        coefficients[start..(start+rows)], values.AsSpan()[start..(start+rows)], 
                        coefficients[pivot], values[pivot], pivot);
                    countDown.Signal();
                });
                offset += rows;
            }

            countDown.Wait();
        }
    }

    private void SubtractFromRows(
        double[][] coefficients, Span<double> values, 
        double[] pivotRow, double pivotValue, int pivot)
    {
        for (int i = 0; i < coefficients.Length; i++)
        {
            double scale = coefficients[i][pivot] / pivotRow[pivot];
            for (int j = pivot+1; j < coefficients[i].Length; j++)
            {
                coefficients[i][j] -= scale * pivotRow[j];
            }

            values[i] -= scale * pivotValue;
            coefficients[i][pivot] = 0;
        }
    }

    private int FindGlobalPivot(Matrix matrix, int start, int end, int column)
    {
        int rowsPerWorker = (end - start) / _threadCount;
        int leftRows = (end - start) % _threadCount;
        int globalPivotRow = start;
        CountdownEvent countDown = new CountdownEvent(_threadCount);
        for (int i = 0; i < _threadCount; i++)
        {
            int rows = rowsPerWorker + (i < leftRows ? 1 : 0);
            if (rows == 0)
            {
                countDown.Signal(_threadCount-i);
                break;
            }
            int startCopy = start;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                int localPivotRow = FindLocalPivot(matrix[startCopy..(startCopy+rows)], column)+startCopy;
                lock (_locker)
                {
                    if (Math.Abs(matrix[globalPivotRow, column]) < Math.Abs(matrix[localPivotRow, column]))
                    {
                        globalPivotRow = localPivotRow;
                    }
                }
                countDown.Signal();
            });
            start += rows;
        }

        countDown.Wait(); //wait for all tasks to finish
        return globalPivotRow;
    }

    private int FindLocalPivot(double[][] rows, int column)
    {
        int maxPivotRow = 0;
        double maxPivot = Math.Abs(rows[0][column]);
        for (int i = 1; i < rows.Length; i++)
        {
            double pivot = Math.Abs(rows[i][column]);
            if (pivot > maxPivot)
            {
                maxPivotRow = i;
                maxPivot = pivot;
            }
        }

        return maxPivotRow;
    }
    
    // process back substitution on an upper triangle matrix
    private void BackwardSubstitution(Matrix coefficients, double[] values)
    {
        int n = values.Length;
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i+1; j < n; j++)
            {
                sum += coefficients[i, j] * values[j];
            }
            values[i] = (values[i] - sum) / coefficients[i, i];
        }
    }
}