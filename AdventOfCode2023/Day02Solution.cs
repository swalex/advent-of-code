using Superpower;
using Superpower.Display;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace AdventOfCode2023;

internal sealed class Day02Solution : ISolution
{
    internal enum Color
    {
        Red,
        Green,
        Blue
    }

    private enum Tokens
    {
        Word,
        Number,
        [Token(Example = ":")]
        Colon,
        [Token(Example = ",")]
        Comma,
        [Token(Example = ";")]
        Semicolon
    }

    private static readonly Tokenizer<Tokens> InputTokenizer = new TokenizerBuilder<Tokens>()
        .Ignore(Span.WhiteSpace)
        .Match(Character.EqualTo(':'), Tokens.Colon)
        .Match(Character.EqualTo(';'), Tokens.Semicolon)
        .Match(Numerics.Natural, Tokens.Number)
        .Match(Character.EqualTo(','), Tokens.Comma)
        .Match(Identifier.CStyle, Tokens.Word)
        .Build();

    private static readonly TokenListParser<Tokens, Color> ColorParser =
        from word in Token.EqualTo(Tokens.Word)
        select word.ToStringValue() switch
        {
            "red" => Color.Red,
            "green" => Color.Green,
            "blue" => Color.Blue,
            _ => throw new InvalidOperationException()
        };

    private static readonly TokenListParser<Tokens, Count> CountParser =
        from number in Token.EqualTo(Tokens.Number)
            .Apply(Numerics.IntegerInt32)
        from color in ColorParser
        select new Count(color, number);

    private static readonly TokenListParser<Tokens, Take> TakeParser =
        from counts in CountParser.ManyDelimitedBy(Token.EqualTo(Tokens.Comma))
        select new Take(counts);

    private static readonly TokenListParser<Tokens, Game> GameParser =
        // skip the "Game" word
        from _ in Token.EqualTo(Tokens.Word)
        from number in Token.EqualTo(Tokens.Number)
            .Apply(Numerics.IntegerInt32)
        from colon in Token.EqualTo(Tokens.Colon)
        from takes in TakeParser.ManyDelimitedBy(Token.EqualTo(Tokens.Semicolon))
        select new Game(number, takes);

    public void Solve()
    {
        string[] input = File.ReadAllLines("InputData/day02.txt");
        Console.WriteLine($"Day  1 - Puzzle 1: {SolvePuzzle1(input, new Take(12, 13, 14))}");
    }

    internal static Game Parse(string line) =>
        GameParser.Parse(InputTokenizer.Tokenize(line));

    internal static int SolvePuzzle1(IEnumerable<string> lines, Take limits) =>
        lines.Select(Parse).Where(g => g.IsPossible(limits)).Select(g => g.Number).Sum();

    internal sealed class Game
    {
        internal Game(int number, IEnumerable<Take> takes)
        {
            Number = number;
            Takes = takes.ToList();
        }

        internal int Number { get; }

        internal int Power =>
            Takes.Select(t => t.Red).Max() *
            Takes.Select(t => t.Green).Max() *
            Takes.Select(t => t.Blue).Max();

        private IReadOnlyList<Take> Takes { get; }

        public override bool Equals(object? obj) =>
            obj is Game other &&
            Number == other.Number &&
            Takes.SequenceEqual(other.Takes);

        public override int GetHashCode() =>
            HashCode.Combine(Number, Takes);

        public override string ToString() =>
            $"{Number}: {string.Join("; ", Takes)}";

        internal bool IsPossible(Take limits) =>
            Takes.All(limits.Within);
    }

    internal readonly record struct Take(int Red, int Green, int Blue)
    {
        internal Take(IEnumerable<Count> counts)
            : this(0, 0, 0)
        {
            foreach (Count count in counts)
            {
                switch (count.Color)
                {
                case Color.Red:
                    Red += count.Amount;
                    break;
                case Color.Green:
                    Green += count.Amount;
                    break;
                case Color.Blue:
                    Blue += count.Amount;
                    break;
                default:
                    throw new InvalidOperationException();
                }
            }
        }

        internal bool Within(Take take) =>
            take.Red <= Red && take.Green <= Green && take.Blue <= Blue;
    }

    internal readonly record struct Count(Color Color, int Amount);
}
