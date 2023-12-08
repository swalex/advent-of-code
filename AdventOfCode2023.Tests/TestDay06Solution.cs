namespace AdventOfCode2023.Tests;

public sealed class TestDay06Solution
{
    private const string ExampleData =
        """
        Time:      7  15   30
        Distance:  9  40  200
        """;

    private static readonly int[] ExpectedOptions = { 4, 8, 9 };

    private static int FirstExpectedResult =>
        ExpectedOptions.Aggregate(1, (a, b) => a * b);

    public static IEnumerable<object[]> EnumerateExpectedOptions() =>
        ExpectedOptions.Select((option, index) => new object[] { index, option });

    [Theory]
    [MemberData(nameof(EnumerateExpectedOptions))]
    public void VerifyExpectedOption(int index, int option)
    {
        List<int> options = Day06Solution.EnumerateOptions(ExampleData.Lines()).ToList();

        Assert.Equal(option, options[index]);
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        int expected = FirstExpectedResult;

        long actual = new Day06Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }
}
