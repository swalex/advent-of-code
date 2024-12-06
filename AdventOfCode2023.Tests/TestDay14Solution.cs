using Xunit.Abstractions;

namespace AdventOfCode2023.Tests;

public sealed class TestDay14Solution
{
    private readonly ITestOutputHelper _helper;

    private const string ExampleData =
        """
        O....#....
        O.OO#....#
        .....##...
        OO.#O....O
        .O.....O#.
        O.#..O.#.#
        ..O..#O..O
        .......O..
        #....###..
        #OO..#....
        """;

    private const long FirstSolutionExpectedResult = 136;

    private const long SecondSolutionExpectedResult = 0;

    public TestDay14Solution(ITestOutputHelper helper)
    {
        _helper = helper ?? throw new ArgumentNullException(nameof(helper));
    }

    [Fact]
    public void VerifyTiltNorth()
    {
        char[,] map = Day14Solution.BuildMap(ExampleData.Lines());
        Day14Solution.TiltNorth(map);

        Day14Solution.DumpMap(map, _helper.AsOutput());
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        var solution = new Day14Solution();

        long actual = solution.SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(FirstSolutionExpectedResult, actual);
    }

    [Fact]
    public void VerifySecondSolution()
    {
        var solution = new Day14Solution();

        long actual = solution.SolveSecondPuzzle(ExampleData.Lines(), _helper.AsOutput());

        Assert.Equal(SecondSolutionExpectedResult, actual);
    }
}
