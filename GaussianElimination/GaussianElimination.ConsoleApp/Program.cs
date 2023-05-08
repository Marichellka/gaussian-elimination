using GaussianElimination.Lib.Algorithms;
using GaussianElimination.Lib.Core;

int[] sizes = new[] { 100, 500, 1000, 1500, 2000, 2500 };
Tester.TestMultiple(new SequentialAlgorithm(), sizes);
Tester.TestCorrectness(new SequentialAlgorithm());