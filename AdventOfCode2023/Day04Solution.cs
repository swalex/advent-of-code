using Superpower;
using Superpower.Display;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace AdventOfCode2023;

public sealed class Day04Solution : ISolution
{
    private enum CardToken
    {
        [Token(Example = "Card")]
        Header,
        [Token(Example = "42")]
        Number,
        [Token(Example = ":")]
        Colon,
        [Token(Example = "|")]
        Pipe
    }

    private static readonly Tokenizer<CardToken> Tokenizer = new TokenizerBuilder<CardToken>()
        .Ignore(Span.WhiteSpace)
        .Match(Span.EqualTo("Card"), CardToken.Header)
        .Match(Numerics.Natural, CardToken.Number)
        .Match(Span.EqualTo(':'), CardToken.Colon)
        .Match(Span.EqualTo('|'), CardToken.Pipe)
        .Build();

    private static readonly TokenListParser<CardToken, int> NumberParser =
        Token.EqualTo(CardToken.Number).Apply(Numerics.IntegerInt32);

    private static readonly TokenListParser<CardToken, int[]> NumbersParser =
        NumberParser.AtLeastOnce();

    private static readonly TokenListParser<CardToken, Card> CardParser =
        from header in Token.EqualTo(CardToken.Header)
        from number in NumberParser
        from winningNumbers in Token.EqualTo(CardToken.Colon).IgnoreThen(NumbersParser)
        from ownedNumbers in Token.EqualTo(CardToken.Pipe).IgnoreThen(NumbersParser)
        select new Card(number, winningNumbers, ownedNumbers);

    public int Day =>
        4;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        input.Select(ParseCard).Select(c => c.Points).Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input)
    {
        IReadOnlyList<Card> cards = input.Select(ParseCard).ToList();
        var multiplier = new int[cards.Count];
        int total = 0;

        foreach (Card card in cards)
        {
            int factor = multiplier[card.Number - 1] + 1;
            total += factor;

            IEnumerable<int> wins = Enumerable.Range(card.Number, card.EnumerateMatchingNumbers().Count());
            foreach (int win in wins)
            {
                multiplier[win] += factor;
            }
        }

        return total;
    }

    internal static Card ParseCard(string line) =>
        CardParser.Parse(Tokenizer.Tokenize(line));

    internal static IEnumerable<int> EnumerateWinningNumbers(string line) =>
        ParseCard(line).EnumerateMatchingNumbers();

    internal readonly record struct Card(int Number, int[] WinningNumbers, int[] OwnedNumbers)
    {
        internal int Points =>
            EnumerateMatchingNumbers().Aggregate(0, (a, _) => a > 0 ? a * 2 : 1);

        internal IEnumerable<int> EnumerateMatchingNumbers() =>
            OwnedNumbers.Where(WinningNumbers.Contains);
    }
}
