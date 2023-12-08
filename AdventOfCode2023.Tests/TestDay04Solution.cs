namespace AdventOfCode2023.Tests;

public sealed class TestDay04Solution
{
    private const string ExampleData =
        """
        Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
        Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
        Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
        Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
        Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
        """;

    private static readonly int[][] WinningNumbers =
    {
        new[] { 48, 83, 17, 86 },
        new[] { 32, 61 },
        new[] { 1, 21 },
        new[] { 84 },
        Array.Empty<int>(),
        Array.Empty<int>()
    };

    public static IEnumerable<object[]> EnumerateWinningNumbers() =>
        WinningNumbers.Select((n, i) => new object[] { i, n });

    [Theory]
    [MemberData(nameof(EnumerateWinningNumbers))]
    public void GetWinningNumbers(int index, int[] expected)
    {
        List<int[]> numbers = ExampleData.Lines()
            .Select(Day04Solution.EnumerateWinningNumbers)
            .Select(n => n.ToArray())
            .ToList();

        Assert.Equal(expected.OrderBy(n => n), numbers[index].OrderBy(n => n));
    }

    [Fact]
    public void Card0Points()
    {
        const int expected = 8;

        int points = Day04Solution.ParseCard(ExampleData.Line(0)).Points;

        Assert.Equal(expected, points);
    }

    [Fact]
    public void Solution1()
    {
        const int expected = 13;

        int actual = new Day04Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Solution2()
    {
        const int expected = 30;

        int actual = new Day04Solution().SolveSecondPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }
}
