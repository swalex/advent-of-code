namespace AdventOfCode2023.Tests;

public sealed class TestDay09Solution
{
    private const string ExampleData =
        """
        0 3 6 9 12 15
        1 3 6 10 15 21
        10 13 16 21 30 45
        """;

    private static readonly int[] ExpectedFirstSubResults = { 18, 28, 68 };

    private static readonly int[] ExpectedSecondSubResults = { -3, 0, 5 };

    private static int ExpectedFirstResult =>
        ExpectedFirstSubResults.Sum();

    private static int ExpectedSecondResult =>
        ExpectedSecondSubResults.Sum();

    public static IEnumerable<object[]> EnumerateFirstSingleExtrapolationData() =>
        ExampleData.Lines().Zip(ExpectedFirstSubResults, (line, expectedSum) => new object[] { line, expectedSum });

    public static IEnumerable<object[]> EnumerateSecondSingleExtrapolationData() =>
        ExampleData.Lines().Zip(ExpectedSecondSubResults, (line, expectedSum) => new object[] { line, expectedSum });

    [Theory]
    [MemberData(nameof(EnumerateFirstSingleExtrapolationData))]
    public void VerifySingleForwardExtrapolation(string line, int expectedSum)
    {
        long actual = Day09Solution.ExtrapolateForward(line);

        Assert.Equal(expectedSum, actual);
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        long actual = new Day09Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(ExpectedFirstResult, actual);
    }

    [Theory]
    [MemberData(nameof(EnumerateSecondSingleExtrapolationData))]
    public void VerifySingleBackwardExtrapolation(string line, int expectedSum)
    {
        long actual = Day09Solution.ExtrapolateBackward(line);

        Assert.Equal(expectedSum, actual);
    }

    [Fact]
    public void VerifySecondSolution()
    {
        long actual = new Day09Solution().SolveSecondPuzzle(ExampleData.Lines());

        Assert.Equal(ExpectedSecondResult, actual);
    }
}
