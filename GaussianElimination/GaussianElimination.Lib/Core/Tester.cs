using System.Diagnostics;
using GaussianElimination.Lib.Algorithms;

namespace GaussianElimination.Lib.Core;

public class Tester
{
    public static void TestMultiple(IAlgorithm algorithm, int[] testsArgs)
    {
        for (int i = 0; i < testsArgs.Length; i++)
        {
            int size = testsArgs[i];
            Matrix coefficients = new Matrix(size, size);
            coefficients.GenerateValues();
            float[] values = new float[size];
            Console.WriteLine($"Start test: {size}");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            algorithm.Solve(coefficients, values);
            watch.Stop();
            Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
        }
    }
}