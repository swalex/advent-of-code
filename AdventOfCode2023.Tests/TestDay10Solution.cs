using System.Drawing;

namespace AdventOfCode2023.Tests;

public sealed class TestDay10Solution
{
    private const string SimpleLoopExampleData =
        """
        -L|F7
        7S-7|
        L|7||
        -L-J|
        L|-JF
        """;

    private const string ComplexLoopExampleData =
        """
        7-F7-
        .FJ|7
        SJLL7
        |F--J
        LJ.LJ
        """;

    private const int SimpleLoopLength = 7;

    private const int ComplexLoopLength = 15;

    private const int SimpleLoopResult = 4;

    private const int ComplexLoopResult = 8;

    private static readonly Point SimpleStartPoint = new(1, 1);

    private static readonly Point ComplexStartPoint = new(0, 2);

    [Fact]
    public void VerifySimpleLoopStart()
    {
        Day10Solution.Map map = Day10Solution.Map.Parse(SimpleLoopExampleData.Lines());

        Assert.Equal(SimpleStartPoint, map.StartPoint);
    }

    [Fact]
    public void VerifySimpleLoopLength()
    {
        Day10Solution.Map map = Day10Solution.Map.Parse(SimpleLoopExampleData.Lines());

        Assert.Equal(SimpleLoopLength, map.FindLength());
    }

    [Fact]
    public void VerifyComplexLoopStart()
    {
        Day10Solution.Map map = Day10Solution.Map.Parse(ComplexLoopExampleData.Lines());

        Assert.Equal(ComplexStartPoint, map.StartPoint);
    }

    [Fact]
    public void VerifyComplexLoopLength()
    {
        Day10Solution.Map map = Day10Solution.Map.Parse(ComplexLoopExampleData.Lines());

        Assert.Equal(ComplexLoopLength, map.FindLength());
    }

    [Fact]
    public void VerifyFirstSolutionOnSimpleLoop()
    {
        long actual = new Day10Solution().SolveFirstPuzzle(SimpleLoopExampleData.Lines());

        Assert.Equal(SimpleLoopResult, actual);
    }

    [Fact]
    public void VerifyFirstSolutionOnComplexLoop()
    {
        long actual = new Day10Solution().SolveFirstPuzzle(ComplexLoopExampleData.Lines());

        Assert.Equal(ComplexLoopResult, actual);
    }
}
