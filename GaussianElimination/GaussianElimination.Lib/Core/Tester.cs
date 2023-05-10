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
            double[] values = coefficients.GenerateValues();
            Console.WriteLine($"Start test: {size}");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            algorithm.Solve(coefficients, values);
            watch.Stop();
            Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
        }
    }

    public static void TestCorrectness(IAlgorithm algorithm, int size)
    {
        Matrix coefficients = new Matrix(size, size);
        double[] values = coefficients.GenerateValues();

        double[] result = algorithm.Solve(coefficients, values);
        
        bool isCorrect = true;

        for (int i = 0; i < result.Length; i++)
        {
            Console.WriteLine($"x{i+1} = {Math.Round(result[i], 4)}");
            if (Math.Abs(result[i] - 1) > 0.0001)
            {
                isCorrect = false;
            }
        }

        Console.WriteLine($"Result is correct: {isCorrect}");
    }

    public static void TestCorrectness(IAlgorithm algorithm)
    {
        Matrix coefficients = new Matrix(new double[][]
        {
            new double[] { 11, 13, -4, 8 },
            new double[] { 1, 9, -5, -3 },
            new double[] { -21, -12, 5, -1 },
            new double[] { 4, 31, 7, 3 },
        });
        double[] values = new double[] { -4, 8, -8, 17 };
        double[] answer = new double[] { 0.22, 0.58, 0.41, -1.54 };
            
        double[] result = algorithm.Solve(coefficients, values);
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