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

    private static readonly int[] Ranks = { 1, 4, 3, 2, 5 };

    private static readonly int[] Bids = { 765, 684, 28, 220, 483 };

    private static IEnumerable<int> OrderedBids =>
        Ranks.Zip(Bids).OrderBy(zip => zip.First).Select(zip => zip.Second);

    private static IEnumerable<int> Winnings =>
        Ranks.Zip(Bids, (r, b) => r * b);

    private static int ExpectedWinnings =>
        Winnings.Sum();

    [Fact]
    public void VerifyHandOrder()
    {
        IEnumerable<Day07Solution.Hand> hands = Day07Solution.EnumerateOrderedHands(ExampleData.Lines());

        Assert.Equal(OrderedBids, hands.Select(h => h.Bid));
    }

    [Fact]
    public void VerifyFirstSolution()
    {
        int expected = ExpectedWinnings;

        long actual = new Day07Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }
}
