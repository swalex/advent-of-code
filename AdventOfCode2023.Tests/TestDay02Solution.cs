namespace AdventOfCode2023.Tests;

public sealed class TestDay02Solution
{
    private const string Test1Input =
        """
        Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
        Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
        Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
        Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
        """;

    private static readonly string[] Test1InputLines = Test1Input.Split("\n");

    [Fact]
    public void TestInputLineCount() =>
        Assert.Equal(5, Test1InputLines.Length);

    [Fact]
    public void TestParser()
    {
        var expected = new Day02Solution.Game(3, new[]
        {
            new Day02Solution.Take(20, 8, 6),
            new Day02Solution.Take(4, 13, 5),
            new Day02Solution.Take(1, 5, 0)
        });

        Day02Solution.Game actual = Day02Solution.Parse(Test1InputLines[2]);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestExample1()
    {
        int actual = Day02Solution.SolvePuzzle1(Test1InputLines, new Day02Solution.Take(12, 13, 14));

        Assert.Equal(8, actual);
    }
}
