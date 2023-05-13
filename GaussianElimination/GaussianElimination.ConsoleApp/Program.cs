using GaussianElimination.Lib.Algorithms;
using GaussianElimination.Lib.Core;

int[] sizes = new[]
{
    500, 1000, 1500, 2000, 2500, 3000
};
int[] threads = new[] { 6, 8, 10, 12, 14};
Tester.TestCorrectness(new RowOrientedAlgorithm(2));
Tester.TestCorrectness(new SequentialAlgorithm(), 500);
Tester.TestCorrectness(new RowOrientedAlgorithm(4), 1000);
Tester.TestMultiple(new SequentialAlgorithm(), sizes);
Tester.TestMultiple(sizes, threads);
