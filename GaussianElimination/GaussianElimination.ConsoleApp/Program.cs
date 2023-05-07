using System.Diagnostics;
using GaussianElimination.Lib.Algorithms;
using GaussianElimination.Lib.Core;

// Matrix coeffs = new Matrix(new float[][]
// {
//     new float[] { 11, 13, -4, 8}, 
//     new float[] { 1, 9, -5, -3}, 
//     new float[] { -21, -12, 5, -1}, 
//     new float[] { 4, 31, 7, 3}, 
// });
// float[] b = new float[]{ -4, 8, -8, 17};

int size = 500;
Matrix coeffs = new Matrix(size, size);
coeffs.GenerateValues();
float[] b = new float[size];
Stopwatch watch = new Stopwatch();
watch.Start();
float[] result = new SequentialAlgorithm().Solve(coeffs, b);
watch.Stop();
Console.WriteLine(watch.ElapsedMilliseconds);
// float[] result = new PartialPivotingAlgorithm().Solve(coeffs, b);
watch.Restart();
result = new SuccessiveAlgorithm(10, 250).Solve(coeffs, b);
watch.Stop();
Console.WriteLine(watch.ElapsedMilliseconds);
// for (int i = 0; i < result.Length; i++)
// {
//     Console.WriteLine($"x{i + 1} = {Math.Round(result[i], 5)}");
// }
