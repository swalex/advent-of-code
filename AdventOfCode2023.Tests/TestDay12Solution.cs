using Xunit.Abstractions;

namespace AdventOfCode2023.Tests;

public sealed class TestDay12Solution
{
    private readonly ITestOutputHelper _helper;

    private const string ExampleData =
        """
        ???.### 1,1,3
        .??..??...?##. 1,1,3
        ?#?#?#?#?#?#?#? 1,3,1,6
        ????.#...#... 4,1,1
        ????.######..#####. 1,6,5
        ?###???????? 3,2,1
        """;

    private static readonly int[] ExpectedArrangementCounts =
    {
        1,
        4,
        1,
        1,
        4,
        10
    };

    public TestDay12Solution(ITestOutputHelper helper)
    {
        _helper = helper ?? throw new ArgumentNullException(nameof(helper));
    }

    private static int ExpectedFirstResult =>
        ExpectedArrangementCounts.Sum();

    public static IEnumerable<object[]> EnumerateExpectedArrangementCounts() =>
        ExpectedArrangementCounts.Select((d, i) => new object[] { ExampleData.Line(i), d });


    [Theory]
    [MemberData(nameof(EnumerateExpectedArrangementCounts))]
    public void DumpVariantsCount(string line, int _)
    {
        _helper.WriteLine(line);
        foreach (string variant in Day12Solution.EnumerateVariants(line))
        {
            _helper.WriteLine(variant);
        }
    }

    [Theory]
    [MemberData(nameof(EnumerateExpectedArrangementCounts))]
    public void VerifyArrangementCount(string line, int expectedCount)
    {
        int actual = Day12Solution.GetArrangementCount(line);

        Assert.Equal(expectedCount, actual);
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        long actual = new Day12Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(ExpectedFirstResult, actual);
    }

    [Fact]
    public void VerifySecondSolution()
    {
        long actual = new Day12Solution().SolveSecondPuzzle(ExampleData.Lines());

        Assert.Equal(0, actual);
    }
}
