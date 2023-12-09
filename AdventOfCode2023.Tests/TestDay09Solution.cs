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

    private static int ExpectedFirstResult =>
        ExpectedFirstSubResults.Sum();

    public static IEnumerable<object[]> EnumerateSingleExtrapolationData() =>
        ExampleData.Lines().Zip(ExpectedFirstSubResults, (line, expectedSum) => new object[] { line, expectedSum });

    [Theory]
    [MemberData(nameof(EnumerateSingleExtrapolationData))]
    public void VerifySingleExtrapolation(string line, int expectedSum)
    {
        long actual = Day09Solution.Extrapolate(line);

        Assert.Equal(expectedSum, actual);
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        long actual = new Day09Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(ExpectedFirstResult, actual);
    }
}
