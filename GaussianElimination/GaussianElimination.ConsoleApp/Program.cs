using GaussianElimination.Lib.Algorithms;
using GaussianElimination.Lib.Core;

Matrix<float> coeffs = new Matrix<float>(new float[][]
{
    new float[] { 2, 1, -1 }, 
    new float[] { -3, -1, 2}, 
    new float[] { -2, 1, 2}
});

float[] values = new float[]{8, -11, -3};
float[] result = new SequentialAlgorithm<float>().Solve(coeffs, values);

for (int i = 0; i < result.Length; i++)
{
    Console.WriteLine($"x{i + 1} = {result[i]}");
}