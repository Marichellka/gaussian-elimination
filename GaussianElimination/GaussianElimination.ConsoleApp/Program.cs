using GaussianElimination.Lib.Algorithms;
using GaussianElimination.Lib.Core;

Matrix coeffs = new Matrix(new float[][]
{
    new float[] { 11, 13, -4, 8}, 
    new float[] { 1, 9, -5, -3}, 
    new float[] { -21, -12, 5, -1}, 
    new float[] { 4, 31, 7, 3}, 
});
float[] values = new float[]{ -4, 8, -8, 17};
float[] result = new SequentialAlgorithm().Solve(coeffs, values);
for (int i = 0; i < result.Length; i++)
{
    Console.WriteLine($"x{i + 1} = {Math.Round(result[i], 5)}");
}