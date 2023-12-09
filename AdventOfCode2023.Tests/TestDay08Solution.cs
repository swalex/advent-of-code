namespace AdventOfCode2023.Tests;

public sealed class TestDay08Solution
{
    private const string ExampleData1 =
        """
        RL
        
        AAA = (BBB, CCC)
        BBB = (DDD, EEE)
        CCC = (ZZZ, GGG)
        DDD = (DDD, DDD)
        EEE = (EEE, EEE)
        GGG = (GGG, GGG)
        ZZZ = (ZZZ, ZZZ)
        """;

    private const string ExampleData2 =
        """
        LLR

        AAA = (BBB, BBB)
        BBB = (AAA, ZZZ)
        ZZZ = (ZZZ, ZZZ)
        """;

    private const string ExampleData3 =
        """
        LR
        
        11A = (11B, XXX)
        11B = (XXX, 11Z)
        11Z = (11B, XXX)
        22A = (22B, XXX)
        22B = (22C, 22C)
        22C = (22Z, 22Z)
        22Z = (22B, 22B)
        XXX = (XXX, XXX)
        """;

    private const int ExpectedStepCount1 = 2;

    private const int ExpectedStepCount2 = 6;

    private const int ExpectedStepCount3 = 6;

    public static IEnumerable<object[]> EnumerateFirstSolutionData()
    {
        yield return new object[] { ExampleData1, ExpectedStepCount1 };
        yield return new object[] { ExampleData2, ExpectedStepCount2 };
    }

    [Theory]
    [MemberData(nameof(EnumerateFirstSolutionData))]
    public void VerifyFirstSolution(string data, int expectedStepCount)
    {
        long actual = new Day08Solution().SolveFirstPuzzle(data.Lines());

        Assert.Equal(expectedStepCount, actual);
    }

    [Fact]
    public void VerifySecondSolution()
    {
        long actual = new Day08Solution().SolveSecondPuzzle(ExampleData3.Lines());

        Assert.Equal(ExpectedStepCount3, actual);
    }
}
