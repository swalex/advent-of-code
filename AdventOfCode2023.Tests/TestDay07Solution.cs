namespace AdventOfCode2023.Tests;

public sealed class TestDay07Solution
{
    private const string ExampleData =
        """
        32T3K 765
        T55J5 684
        KK677 28
        KTJJT 220
        QQQJA 483
        """;

    private static readonly int[] FirstPuzzleRanks = { 1, 4, 3, 2, 5 };

    private static readonly int[] SecondPuzzleRanks = { 1, 3, 2, 5, 4 };

    private static readonly int[] Bids = { 765, 684, 28, 220, 483 };

    private static IEnumerable<int> Order(IEnumerable<int> ranks) =>
        ranks.Zip(Bids).OrderBy(zip => zip.First).Select(zip => zip.Second);

    private static IEnumerable<int> Winnings(IEnumerable<int> ranks) =>
        ranks.Zip(Bids, (r, b) => r * b);

    private static int ExpectedFirstWinnings =>
        Winnings(FirstPuzzleRanks).Sum();

    private static int ExpectedSecondWinnings =>
        Winnings(SecondPuzzleRanks).Sum();

    [Fact]
    public void VerifyFirstHandOrder()
    {
        IEnumerable<Day07Solution.Hand> hands = Day07Solution.EnumerateOrderedHands(ExampleData.Lines());

        Assert.Equal(Order(FirstPuzzleRanks), hands.Select(h => h.Bid));
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        int expected = ExpectedFirstWinnings;

        long actual = new Day07Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void VerifySecondHandOrder()
    {
        IEnumerable<Day07Solution.Hand> hands = Day07Solution.EnumerateOrderedHandsWithJoker(ExampleData.Lines());

        Assert.Equal(Order(SecondPuzzleRanks), hands.Select(h => h.Bid));
    }

    [Fact]
    public void VerifySecondSolution()
    {
        long expected = ExpectedSecondWinnings;

        long actual = new Day07Solution().SolveSecondPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }
}
