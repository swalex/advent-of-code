namespace AdventOfCode2023.Tests;

public sealed class TestDay03Solution
{
    private const string ExampleData =
        """
        467..114..
        ...*......
        ..35..633.
        ......#...
        617*......
        .....+.58.
        ..592.....
        ......755.
        ...$.*....
        .664.598..
        """;

    [Fact]
    public void Solution1()
    {
        const int expected = 4361;

        int actual = new Day03Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EnumerateNumbers()
    {
        const int expected = 10;

        List<Day03Solution.Map.Number> numbers = Day03Solution.GetMap(ExampleData.Lines()).EnumerateNumbers().ToList();

        Assert.Equal(expected, numbers.Count);
    }
}
