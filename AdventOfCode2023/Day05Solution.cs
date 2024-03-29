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

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        SolveFirstPuzzle(ParseAlmanac(input));

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        SolveSecondPuzzle(ParseAlmanac(input));

    internal static Almanac ParseAlmanac(IEnumerable<string> lines) =>
        TheParser.Parse(lines);

    private static long SolveFirstPuzzle(Almanac almanac) =>
        almanac.Seeds.Select(almanac.GetLocationForSeed).Min();

    private static long BlowUpWhileSolvingSecondPuzzle(Almanac almanac) =>
        almanac.EnumerateAsRanges().Select(almanac.GetLocationForSeed).Min();

    private static long SolveSecondPuzzle(Almanac almanac)
    {
        RangeMap seedsMap = almanac.BuildSeedsMap();
        RangeMap compiledMap = almanac.CompileMaps();
        RangeMap mergedMap = seedsMap.MergeWith(compiledMap);

        return mergedMap.Ranges
            .Where(r => seedsMap.Contains(r.SourceStart))
            .Select(r => r.DestinationStart)
            .Min();
    }

    internal sealed record Range(long DestinationStart, long SourceStart, long Length)
    {
        internal IEnumerable<long> EnumerateDestinationPoints()
        {
            yield return DestinationStart;
            yield return DestinationEnd;
        }

        internal IEnumerable<long> EnumerateSourcePoints()
        {
            yield return SourceStart;
            yield return SourceEnd;
        }

        internal long DestinationEnd =>
            DestinationStart + Length;

        internal long SourceEnd =>
            SourceStart + Length;

        internal bool ContainsDestination(long value) =>
            DestinationStart <= value && value < DestinationStart + Length;

        internal bool ContainsSource(long value) =>
            SourceStart <= value && value < SourceStart + Length;

        internal long Resolve(long value) =>
            value - SourceStart + DestinationStart;

        internal long Reverse(long value) =>
            value - DestinationStart + SourceStart;
    }

    internal sealed record RangeMap(string From, string To, Range[] Ranges)
    {
        internal bool Contains(long value) =>
            Ranges.Any(r => r.ContainsSource(value));

        internal RangeMap MergeWith(RangeMap other)
        {
            if (!To.Equals(other.From)) throw new InvalidOperationException();

            List<long> intersections = Intersect(Ranges, other.Ranges).ToList();
            Range[] ranges = intersections.SkipLast(1)
                .Select((p, i) => new Range(other.Resolve(p), Reverse(p), intersections[i + 1] - p))
                .ToArray();

            return new RangeMap(From, other.To, ranges);
        }

        private long Reverse(long value) =>
            Ranges.SingleOrDefault(r => r.ContainsDestination(value))?.Reverse(value) ?? value;

        internal long Resolve(long value) =>
            Ranges.SingleOrDefault(r => r.ContainsSource(value))?.Resolve(value) ?? value;

        private static IEnumerable<long> Intersect(IEnumerable<Range> front, IEnumerable<Range> back) =>
            front
                .SelectMany(r => r.EnumerateDestinationPoints())
                .Concat(back.SelectMany(r => r.EnumerateSourcePoints()))
                .Distinct()
                .OrderBy(p => p);
    }

    internal sealed record Almanac(long[] Seeds, RangeMap[] Maps)
    {
        internal IEnumerable<long> EnumerateAsRanges() =>
            Seeds.Chunk(2).SelectMany(AsRange);

        internal long GetLocationForSeed(long seed) =>
            GetValueForSeed("location", seed);

        internal long GetValueByIndex(string key, long index) =>
            GetValueForSeed(key, Seeds[index]);

        internal RangeMap CompileMaps() =>
            Maps.Aggregate(default(RangeMap), (compiled, map) => compiled is null ? map : compiled.MergeWith(map)) ??
            throw new InvalidOperationException();

        internal RangeMap BuildSeedsMap() =>
            new("love", "seed", Seeds.Chunk(2).Select(s => new Range(s[0], s[0], s[1])).ToArray());

        private static IEnumerable<long> AsRange(long[] chunk)
        {
            for (long i = 0; i < chunk[1]; i++) yield return chunk[0] + i;
        }

        private long GetValueForSeed(string key, long value)
        {
            var step = "seed";
            do
            {
                // Console.Write($"{step} #{value} -> ");
                RangeMap map = GetMapFor(step);
                value = map.Resolve(value);
                step = map.To;
                // Console.WriteLine($"{value}");
            } while (!key.Equals(step));

            return value;
        }

        private RangeMap GetMapFor(string step) =>
            Maps.Single(m => step.Equals(m.From));
    }

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

        private static readonly TokenListParser<AlmanacTokens, long> NumberParser =
            Token.EqualTo(AlmanacTokens.Number).Apply(Numerics.IntegerInt64);

        private static readonly TokenListParser<AlmanacTokens, long[]> NumbersParser =
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

        private static readonly TokenListParser<AlmanacTokens, long[]> SeedsParser =
            from _ in Token.EqualToValue(AlmanacTokens.Identifier, "seeds")
            from __ in Token.EqualTo(AlmanacTokens.Colon)
            from seeds in NumbersParser
            select seeds;

        private static readonly TokenListParser<AlmanacTokens, Almanac> AlmanacParser =
            from seeds in SeedsParser
            from maps in RangeMapParser.Many()
            select new Almanac(seeds, maps);

        internal static Almanac Parse(IEnumerable<string> input) =>
            Parse(string.Join('\n', input));

        private static Almanac Parse(string input) =>
            AlmanacParser.Parse(Tokenizer.Tokenize(input));
    }
}
