namespace AdventOfCode2023.Tests;

public sealed class TestDay01Solution
{
    [Fact]
    public void TestConvertDigits()
    {
        const string input = "eightjzqzhrllg1oneightfck";

        string digits = Day01Solution.ConvertDigits(input);

        Assert.Equal("88", digits);
    }
}
