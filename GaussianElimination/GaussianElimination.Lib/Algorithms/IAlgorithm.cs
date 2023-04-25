using System.Numerics;
using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public interface IAlgorithm<T> where T: 
    IMultiplyOperators<T, T, T>, ISubtractionOperators<T, T, T>,
    IDivisionOperators<T, T, T>, IAdditionOperators<T, T, T>
{
    public T[] Solve(Matrix<T> coefficients, T[] values);
}