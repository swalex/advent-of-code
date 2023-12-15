namespace AdventOfCode2023.Tests;

public sealed class TestDay15Solution
{
    private const string TestSequence = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";

    [Fact]
    public void VerifyFancyHashOnFixedString()
    {
        const int expectedValue = 52;
        const string input = "HASH";

        int actualValue = input.FancyHash();

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        const long expectedResult = 1320;
        var solution = new Day15Solution();

        long actual = solution.SolveFirstPuzzle(TestSequence.Lines());

        Assert.Equal(expectedResult, actual);
    }

    [Fact]
    public void VerifySecondSolution()
    {
        const long expectedResult = 145;
        var solution = new Day15Solution();

        long actual = solution.SolveSecondPuzzle(TestSequence.Lines());

        Assert.Equal(expectedResult, actual);
    }
}
