namespace AdventOfCode2023.Tests;

public sealed class TestDay11Solution
{
    private const string ExampleData =
        """
        ...#......
        .......#..
        #.........
        ..........
        ......#...
        .#........
        .........#
        ..........
        .......#..
        #...#.....
        """;

    private static readonly (int, int, int)[] Distances =
    {
        (1, 7, 15),
        (3, 6, 17),
        (5, 9, 9),
        (8, 9, 5)
    };

    private static readonly (int, int)[] ExpectedScaledResults =
    {
        (10, 1030),
        (100, 8410)
    };

    private const int ExpectedFirstResult = 374;

    public static IEnumerable<object[]> EnumerateDistances() =>
        Distances.Select(d => new object[] { d.Item1, d.Item2, d.Item3 });

    public static IEnumerable<object[]> EnumerateExpectedScaledResults() =>
        ExpectedScaledResults.Select(d => new object[] { d.Item1, d.Item2 });

    [Theory]
    [MemberData(nameof(EnumerateDistances))]
    public void VerifyDistance(int x, int y, int expectedDistance)
    {
        Day11Solution.Map map = Day11Solution.Map.Parse(ExampleData.Lines());

        long actual = map.GetDistance(x, y);

        Assert.Equal(expectedDistance, actual);
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        long actual = new Day11Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(ExpectedFirstResult, actual);
    }

    [Theory]
    [MemberData(nameof(EnumerateExpectedScaledResults))]
    public void VerifySecondSolution(int scale, int expectedDistance)
    {
        Day11Solution.Map map = Day11Solution.Map.Parse(ExampleData.Lines());

        long actual = map.GetResult(scale);

        Assert.Equal(expectedDistance, actual);
    }
}
