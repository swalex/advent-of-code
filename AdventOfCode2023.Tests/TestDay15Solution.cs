namespace AdventOfCode2023.Tests;

public sealed class TestDay15Solution
{
    private const string TestSequence = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";

    [Fact]
    public void VerifyFancyHashOnFixedString()
    {
        const int expectedValue = 52;
        const string input = "HASH";
        Day15Solution.FancyHash hash = new();

        byte actualValue = hash.Add(input);

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
}
