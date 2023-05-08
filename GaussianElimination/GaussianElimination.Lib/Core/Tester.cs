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

    public static void TestCorrectness(IAlgorithm algorithm)
    {
        Matrix coefficients = new Matrix(new float[][]
        {
            new float[] { 11, 13, -4, 8 },
            new float[] { 1, 9, -5, -3 },
            new float[] { -21, -12, 5, -1 },
            new float[] { 4, 31, 7, 3 },
        });
        float[] values = new float[] { -4, 8, -8, 17 };
        double[] answer = new double[] { 0.22, 0.58, 0.41, -1.54 };
            
        float[] result = algorithm.Solve(coefficients, values);
        bool isCorrect = true;

        for (int i = 0; i < result.Length; i++)
        {
            Console.WriteLine($"x{i+1} = {result[i]}");
            if (Math.Abs(result[i] - answer[i]) > 0.01)
            {
                isCorrect = false;
            }
        }

        Console.WriteLine($"Result is correct: {isCorrect}");
    }
}