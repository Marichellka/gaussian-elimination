using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public interface IAlgorithm
{
    public float[] Solve(Matrix<float> coefficients, float[] values);
}