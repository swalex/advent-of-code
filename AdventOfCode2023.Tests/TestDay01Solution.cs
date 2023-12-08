namespace AdventOfCode2023.Tests;

public sealed class TestDay01Solution
{
    private const string Example1Data =
        """
        1abc2
        pqr3stu8vwx
        a1b2c3d4e5f
        treb7uchet
        """;

    private const string Example2Data =
        """
        two1nine
        eightwothree
        abcone2threexyz
        xtwone3four
        4nineeightseven2
        zoneight234
        7pqrstsixteen
        """;

    private static readonly int[] ExpectedExample1Values = { 12, 38, 15, 77 };

    public static IEnumerable<object[]> GetExample1Values() =>
        Example1Data.Lines().Select((l, i) => new object[] { l, ExpectedExample1Values[i] });

    [Fact]
    public void ConvertDigits()
    {
        const string input = "eightjzqzhrllg1oneightfck";

        string digits = Day01Solution.ConvertDigits(input);

        Assert.Equal("88", digits);
    }

    [Theory]
    [MemberData(nameof(GetExample1Values))]
    public void Solution1LineResults(string line, int expectedValue)
    {
        string actual = Day01Solution.ConvertDigits(line);

        Assert.Equal(expectedValue.ToString("D2"), actual);
    }

    [Fact]
    public void Solution1Result()
    {
        const long expected = 142;

        long actual = new Day01Solution().SolveFirstPuzzle(Example1Data.Lines());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Solution2Result()
    {
        const long expected = 281;

        long actual = new Day01Solution().SolveSecondPuzzle(Example2Data.Lines());

        Assert.Equal(expected, actual);
    }
}
