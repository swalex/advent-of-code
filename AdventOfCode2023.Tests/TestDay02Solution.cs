namespace AdventOfCode2023.Tests;

public sealed class TestDay02Solution
{
    private const string ExampleData =
        """
        Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
        Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
        Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
        Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
        """;

    private static IReadOnlyList<string> ExampleDataLines =>
        ExampleData.Lines();

    private static readonly int[] ExpectedPower = { 48, 12, 1560, 630, 36 };

    public static IEnumerable<object[]> GetPowerCombinations() =>
        ExampleDataLines.Select((l, i) => new object[] { l, ExpectedPower[i] });

    [Fact]
    public void TestInputLineCount() =>
        Assert.Equal(5, ExampleDataLines.Count);

    [Fact]
    public void TestParser()
    {
        var expected = new Day02Solution.Game(3, new[]
        {
            new Day02Solution.Take(20, 8, 6),
            new Day02Solution.Take(4, 13, 5),
            new Day02Solution.Take(1, 5, 0)
        });

        Day02Solution.Game actual = Day02Solution.Parse(ExampleDataLines[2]);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestExample1()
    {
        Day02Solution.Take limits = new Day02Solution.Take(12, 13, 14);
        int actual = ExampleDataLines.Select(Day02Solution.Parse).Where(g => g.IsPossible(limits)).Select(g => g.Number).Sum();

        Assert.Equal(8, actual);
    }

    [Theory]
    [MemberData(nameof(GetPowerCombinations))]
    public void TestPowerOfGame3(string line, int expectedPower)
    {
        Day02Solution.Game actual = Day02Solution.Parse(line);

        Assert.Equal(expectedPower, actual.Power);
    }
}
