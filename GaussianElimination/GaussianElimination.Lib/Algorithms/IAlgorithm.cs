using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public interface IAlgorithm
{
    public double[] Solve(Matrix coefficients, double[] values);
}