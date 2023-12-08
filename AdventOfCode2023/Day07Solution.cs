namespace AdventOfCode2023;

public sealed class Day07Solution : ISolution
{
    private const string FirstDeck = "AKQJT98765432";

    private const string SecondDeck = "AKQT98765432J";

    public int Day =>
        7;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        EnumerateOrderedHands(input)
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        EnumerateOrderedHandsWithJoker(input)
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();

    internal static IEnumerable<Hand> EnumerateOrderedHands(IEnumerable<string> lines) =>
        lines
            .Select(ParseHand)
            .GroupBy(HandType)
            .OrderBy(g => g.Key)
            .SelectMany(g => g.OrderDescending(BySecondOrdering.WithoutJoker));

    internal static IEnumerable<Hand> EnumerateOrderedHandsWithJoker(IEnumerable<string> lines) =>
        lines
            .Select(ParseHand)
            .GroupBy(HandTypeWithJoker)
            .OrderBy(g => g.Key)
            .SelectMany(g => g.OrderDescending(BySecondOrdering.WithJoker));

    private sealed class BySecondOrdering : IComparer<Hand>
    {
        private readonly string _deck;

        private BySecondOrdering(string deck)
        {
            _deck = deck;
        }

        internal static IComparer<Hand> WithoutJoker { get; } = new BySecondOrdering(FirstDeck);

        internal static IComparer<Hand> WithJoker { get; } = new BySecondOrdering(SecondDeck);

        public int Compare(Hand x, Hand y) =>
            x.EnumerateHandValues(_deck).Zip(y.EnumerateHandValues(_deck), (a, b) => a.CompareTo(b))
                .FirstOrDefault(c => c != 0);
    }

    private static Hand ParseHand(string line)
    {
        string[] parts = line.Split(' ');
        return new Hand(parts[0], int.Parse(parts[1]));
    }

    private static Hand.Type HandType(Hand hand) =>
        hand.GetHandType();

    private static Hand.Type HandTypeWithJoker(Hand hand) =>
        hand.GetHandTypeWithJoker();

    internal readonly record struct Hand(string Cards, int Bid)
    {
        internal IEnumerable<int> EnumerateHandValues(string deck) =>
            Cards.Select(card => deck.IndexOf(card));

        internal Type GetHandTypeWithJoker() =>
            GetHandType(Cards.Replace('J', Pretend(Cards)));

        private static char Pretend(IEnumerable<char> cards) =>
            cards.Where(c => c != 'J').GroupBy(c => c).MaxBy(g => g.Count())?.Key ?? 'J';

        internal Type GetHandType() =>
            GetHandType(Cards);

        private static Type GetHandType(IEnumerable<char> cards)
        {
            List<IGrouping<char, char>> groups = cards.GroupBy(c => c).ToList();
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
