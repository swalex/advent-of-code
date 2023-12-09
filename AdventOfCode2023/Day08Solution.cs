using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace AdventOfCode2023;

public sealed class Day08Solution : ISolution
{
    public int Day =>
        8;

    public long SolveFirstPuzzle(IReadOnlyList<string> input)
    {
        (int[] directions, (string, string, string)[] nodes) data = TheParser.Parse(input);
        Dictionary<string, (string, string)> map = data.nodes.ToDictionary(d => d.Item1, d => (d.Item2, d.Item3));

        var current = "AAA";
        const string last = "ZZZ";

        var step = 0L;
        var i = 0;
        while (current != last)
        {
            (string left, string right) = map[current];
            current = data.directions[i++] == 0 ? left : right;
            step++;

            if (i == data.directions.Length) i = 0;
        }

        return step;
    }

    public long FailToSolveSecondPuzzle(IReadOnlyList<string> input)
    {
        (int[] directions, (string, string, string)[] nodes) data = TheParser.Parse(input);
        Dictionary<string, (string, string)> map = data.nodes.ToDictionary(d => d.Item1, d => (d.Item2, d.Item3));

        string[] positions = data.nodes.Select(n => n.Item1).Where(n => n[2] == 'A').ToArray();

        Func<(string, string), string> leftSelector = x => x.Item1;
        Func<(string, string), string> rightSelector = x => x.Item2;

        var step = 0L;
        var i = 0;
        while (positions.Any(p => p[2] != 'Z'))
        {
            Func<(string, string), string> selector = data.directions[i] == 0 ? leftSelector : rightSelector;

            for (var j = 0; j < positions.Length; j++)
            {
                positions[j] = selector.Invoke(map[positions[j]]);
            }

            i++;
            step++;
            if (i == data.directions.Length) i = 0;

            if (step % 1000000 == 0) Console.Write('.');
            if (step % 100000000 == 0) Console.WriteLine();
        }

        return step;
    }

    public long SolveSecondPuzzle(IReadOnlyList<string> input)
    {
        (int[] directions, (string, string, string)[] nodes) data = TheParser.Parse(input);
        Dictionary<string, (string, string)> map = data.nodes.ToDictionary(d => d.Item1, d => (d.Item2, d.Item3));

        string[] positions = data.nodes.Select(n => n.Item1).Where(n => n[2] == 'A').ToArray();

        Func<(string, string), string> leftSelector = x => x.Item1;
        Func<(string, string), string> rightSelector = x => x.Item2;

        long[] steps = positions.Select(_ => 0L).ToArray();

        for (var j = 0; j < positions.Length; j++)
        {
            var step = 0L;
            var i = 0;

            while (positions[j][2] != 'Z')
            {
                Func<(string, string), string> selector = data.directions[i] == 0 ? leftSelector : rightSelector;

                    positions[j] = selector.Invoke(map[positions[j]]);

                i++;
                step++;
                if (i == data.directions.Length) i = 0;
            }

            steps[j] = step;
        }

        Console.WriteLine(string.Join(", ", steps.Select(s => s.ToString())));
        return LeastCommonMultiple(steps);
    }

    private static long LeastCommonMultiple(IEnumerable<long> numbers) =>
        numbers.Aggregate(LeastCommonMultiple);

    private static long LeastCommonMultiple(long a, long b) =>
        Math.Abs(a * b) / GreatestCommonDivisor(a, b);

    private static long GreatestCommonDivisor(long a, long b)
    {
        while (true)
        {
            if (b == 0) return a;

            long tmp = a;
            a = b;
            b = tmp % b;
        }
    }

    private static class TheParser
    {
        private enum MapToken
        {
            Letter,
            Equal,
            OpenBrace,
            Comma,
            CloseBrace,
            Semicolon
        }

        private static readonly Tokenizer<MapToken> Tokenizer = new TokenizerBuilder<MapToken>()
            .Ignore(Character.WhiteSpace)
            .Match(Character.Upper.Or(Character.Digit), MapToken.Letter)
            .Match(Character.EqualToIgnoreCase('='), MapToken.Equal)
            .Match(Character.EqualTo('('), MapToken.OpenBrace)
            .Match(Character.EqualTo(','), MapToken.Comma)
            .Match(Character.EqualTo(')'), MapToken.CloseBrace)
            .Match(Character.EqualTo(';'), MapToken.Semicolon)
            .Build();

        private static readonly TokenListParser<MapToken, int> DirectionParser =
            Token.EqualToValue(MapToken.Letter, "L").Value(0).Or(Token.EqualToValue(MapToken.Letter, "R").Value(1));

        private static readonly TokenListParser<MapToken, int[]> DirectionsParser =
            from directions in DirectionParser.Many()
            from _ in Token.EqualTo(MapToken.Semicolon).Many()
            select directions;

        private static readonly TokenListParser<MapToken, string> LocationParser =
            Token.EqualTo(MapToken.Letter).Apply(Character.Upper.Or(Character.Digit)).Repeat(3).Select(c => new string(c));

        private static readonly TokenListParser<MapToken, (string location, string left, string right)> NodeParser =
            from location in LocationParser
            from _ in Token.EqualTo(MapToken.Equal).IgnoreThen(Token.EqualTo(MapToken.OpenBrace))
            from left in LocationParser
            from right in Token.EqualTo(MapToken.Comma).IgnoreThen(LocationParser)
            from __ in Token.EqualTo(MapToken.CloseBrace).IgnoreThen(Token.EqualTo(MapToken.Semicolon))
            select (location, left, right);

        private static readonly TokenListParser<MapToken, (string, string, string)[]> NodesParser =
            NodeParser.Many();

        private static readonly TokenListParser<MapToken, (int[] directions, (string, string, string)[] nodes)> MapParser =
                from directions in DirectionsParser
                from nodes in NodesParser
                select (directions, nodes);

        internal static (int[] directions, (string, string, string)[] nodes) Parse(IEnumerable<string> input) =>
            MapParser.Parse(Tokenizer.Tokenize(string.Join(";\n", input) + ';'));
    }
}
