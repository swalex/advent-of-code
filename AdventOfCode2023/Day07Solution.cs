namespace AdventOfCode2023;

public sealed class Day07Solution : ISolution
{
    public int Day =>
        7;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        EnumerateOrderedHands(input)
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    internal static IEnumerable<Hand> EnumerateOrderedHands(IEnumerable<string> lines) =>
        lines
            .Select(ParseHand)
            .GroupBy(HandType)
            .OrderBy(g => g.Key)
            .SelectMany(g => g.OrderDescending(BySecond.Ordering));

    private sealed class BySecond : IComparer<Hand>
    {
        internal static IComparer<Hand> Ordering { get; } = new BySecond();

        public int Compare(Hand x, Hand y) =>
            x.EnumerateHandValues().Zip(y.EnumerateHandValues(), (a, b) => a.CompareTo(b))
                .FirstOrDefault(c => c != 0);
    }

    private static Hand ParseHand(string line)
    {
        string[] parts = line.Split(' ');
        return new Hand(parts[0], int.Parse(parts[1]));
    }

    private static Hand.Type HandType(Hand hand) =>
        hand.GetHandType();

    internal readonly record struct Hand(string Cards, int Bid)
    {
        private const string Deck = "AKQJT98765432";

        internal IEnumerable<int> EnumerateHandValues() =>
            Cards.Select(card => Deck.IndexOf(card));

        internal Type GetHandType()
        {
            List<IGrouping<char, char>> groups = Cards.GroupBy(c => c).ToList();
            return groups.Count switch
            {
                5 => Type.HighCard,
                4 => Type.OnePair,
                3 => groups.Any(g => g.Count() == 3) ? Type.ThreeOfAKind : Type.TwoPairs,
                2 => groups.Any(g => g.Count() == 4) ? Type.FourOfAKind : Type.FullHouse,
                1 => Type.FiveOfAKind,
                _ => throw new InvalidOperationException()
            };
        }

        internal enum Type
        {
            HighCard,

            OnePair,

            TwoPairs,

            ThreeOfAKind,

            FullHouse,

            FourOfAKind,

            FiveOfAKind
        }
    }
}
