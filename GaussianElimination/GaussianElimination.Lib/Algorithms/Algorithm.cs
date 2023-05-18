using GaussianElimination.Lib.Core;

namespace GaussianElimination.Lib.Algorithms;

public abstract class Algorithm
{
    protected const double Accuracy = 0.0001;
    public abstract double[] Solve(Matrix coefficients, double[] values);

    protected void ValidateSystem(Matrix coefficients, double[] values)
    {
        if (coefficients.Length != values.Length)
        {
            throw new ArgumentException(
                $"Count of equations {coefficients.Length} isn't equal to count of values {values.Length}");
        }

        if (!coefficients.IsSquare())
        {
            throw new ArgumentException(
                $"Matrix of coefficients isn't square");
        }
    }

    protected void ValidatePivot(double pivot)
    {
        if (Math.Abs(pivot) <= Accuracy)
        {
            throw new ArgumentException("System has infinitely many solutions");
        }
    }
}