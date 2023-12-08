using Superpower;
using Superpower.Display;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace AdventOfCode2023;

public sealed class Day05Solution : ISolution
{
    public int Day =>
        5;

    public int SolveFirstPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    public int SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    internal static Almanac ParseAlmanac(IEnumerable<string> lines) =>
        TheParser.Parse(string.Join('\n', lines));

    internal readonly record struct Range(int DestinationStart, int SourceStart, int Length);

    internal readonly record struct RangeMap(string From, string To, Range[] Ranges);

    internal readonly record struct Almanac(int[] Seeds, RangeMap[] Maps);

    private static class TheParser
    {
        private enum AlmanacTokens
        {
            [Token(Example = "seeds")]
            Identifier,

            [Token(Example = ":")]
            Colon,

            [Token(Example = "-")]
            Dash,

            [Token(Example = "42")]
            Number
        }

        private static readonly Tokenizer<AlmanacTokens> Tokenizer = new TokenizerBuilder<AlmanacTokens>()
            .Ignore(Span.WhiteSpace)
            .Match(Identifier.CStyle, AlmanacTokens.Identifier)
            .Match(Span.EqualTo(":"), AlmanacTokens.Colon)
            .Match(Span.EqualTo("-"), AlmanacTokens.Dash)
            .Match(Numerics.Natural, AlmanacTokens.Number)
            .Build();

        private static readonly TokenListParser<AlmanacTokens, int> NumberParser =
            Token.EqualTo(AlmanacTokens.Number).Apply(Numerics.IntegerInt32);

        private static readonly TokenListParser<AlmanacTokens, int[]> NumbersParser =
            NumberParser.Many();

        private static readonly TokenListParser<AlmanacTokens, Range> RangeParser =
            from destination in NumberParser
            from source in NumberParser
            from length in NumberParser
            select new Range(destination, source, length);

        private static readonly TokenListParser<AlmanacTokens, Range[]> RangesParser =
            RangeParser.Many();

        private static readonly TokenListParser<AlmanacTokens, string> IdentifierParser =
            from identifier in Token.EqualTo(AlmanacTokens.Identifier)
            select identifier.ToStringValue();

        private static readonly TokenListParser<AlmanacTokens, Unit> SeparatorParser =
            from _ in Token.EqualTo(AlmanacTokens.Dash)
            from __ in Token.EqualToValue(AlmanacTokens.Identifier, "to")
            from ___ in Token.EqualTo(AlmanacTokens.Dash)
            select Unit.Value;

        private static readonly TokenListParser<AlmanacTokens, (string From, string To)> MapHeaderParser =
            from source in IdentifierParser
            from _ in SeparatorParser
            from destination in IdentifierParser
            from __ in Token.EqualToValue(AlmanacTokens.Identifier, "map")
            from ___ in Token.EqualTo(AlmanacTokens.Colon)
            select (source, destination);

        private static readonly TokenListParser<AlmanacTokens, RangeMap> RangeMapParser =
            from header in MapHeaderParser
            from ranges in RangesParser
            select new RangeMap(header.From, header.To, ranges);

        private static readonly TokenListParser<AlmanacTokens, int[]> SeedsParser =
            from _ in Token.EqualToValue(AlmanacTokens.Identifier, "seeds")
            from __ in Token.EqualTo(AlmanacTokens.Colon)
            from seeds in NumbersParser
            select seeds;

        private static readonly TokenListParser<AlmanacTokens, Almanac> AlmanacParser =
            from seeds in SeedsParser
            from maps in RangeMapParser.Many()
            select new Almanac(seeds, maps);

        internal static Almanac Parse(string input) =>
            AlmanacParser.Parse(Tokenizer.Tokenize(input));
    }
}
