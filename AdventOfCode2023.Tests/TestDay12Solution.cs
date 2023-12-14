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

    private static readonly int[] ExpectedUnfoldedArrangementCounts =
    {
        1,
        16384,
        1,
        16,
        2500,
        506250
    };

    private static readonly Dictionary<string, (int, (int, int)[])> AdditionalUnfoldedTests = new()
    {
        { "?? 1", (2, new[] { (5, 252), (2, 6), (3, 20) }) },
        { "???? 1,1", (3, new[] { (5, 3003) }) },
    };

    public TestDay12Solution(ITestOutputHelper helper)
    {
        _helper = helper ?? throw new ArgumentNullException(nameof(helper));
    }

    private static int ExpectedFirstResult =>
        ExpectedArrangementCounts.Sum();

    public static IEnumerable<object[]> EnumerateExpectedArrangementCounts() =>
        ExpectedArrangementCounts.Select((d, i) => new object[] { ExampleData.Line(i), d })
            .Concat(AdditionalUnfoldedTests.Select(entry => new object[] { entry.Key, entry.Value.Item1 }));

    public static IEnumerable<object[]> EnumerateExpectedUnfoldedArrangementCounts() =>
        ExpectedUnfoldedArrangementCounts.Select((d, i) => new object[] { ExampleData.Line(i), d })
            .Concat(AdditionalUnfoldedTests.Select(entry => new object[] { entry.Key, entry.Value.Item2[0].Item2 }));

    public static IEnumerable<object[]> EnumerateAdditionalUnfoldedArrangementCounts() =>
        AdditionalUnfoldedTests.SelectMany(entry => entry.Value.Item2.Select(subEntry =>
            new object[] { entry.Key, subEntry.Item1, subEntry.Item2 }));

    [Theory]
    [MemberData(nameof(EnumerateExpectedArrangementCounts))]
    public void DumpVariantsCount(string line, int expectedCount)
    {
        _helper.WriteLine($"{line} ({expectedCount})");
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

    [Theory]
    [MemberData(nameof(EnumerateExpectedUnfoldedArrangementCounts))]
    public void VerifyUnfoldedArrangementCount(string line, int expectedCount)
    {
        long actual = Day12Solution.GetUnfoldedArrangementCount(line);

        Assert.Equal(expectedCount, actual);
    }

    [Theory]
    [MemberData(nameof(EnumerateAdditionalUnfoldedArrangementCounts))]
    public void VerifyAdditionalUnfoldedArrangementCount(string line, int folds, int expectedCount)
    {
        long actual = Day12Solution.GetUnfoldedArrangementCount(line, folds);

        Assert.Equal(expectedCount, actual);
    }

    [Theory]
    [MemberData(nameof(EnumerateAdditionalUnfoldedArrangementCounts))]
    public void VerifyBruteForceUnfoldedArrangementCount(string line, int folds, int expectedCount)
    {
        long actual = Day12Solution.BruteForceUnfoldedArrangementCount(line, folds);

        Assert.Equal(expectedCount, actual);
    }

    [Fact]
    public void VerifySecondSolution()
    {
        long actual = new Day12Solution().SolveSecondPuzzle(ExampleData.Lines());

        Assert.Equal(525152, actual);
    }

    [Fact]
    public void FuckEverythingSucks()
    {
        const string line = "??????????#.. 6,1";
        const int expectedCount = 4;

        long actual = Day12Solution.GetArrangementCount(line);

        Assert.Equal(expectedCount, actual);
    }
}
