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

        var current = "AAA";
        const string last = "ZZZ";
        Dictionary<string, (string, string)> map = data.nodes.ToDictionary(d => d.Item1, d => (d.Item2, d.Item3));

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

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

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
            .Match(Character.Upper, MapToken.Letter)
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
            Token.EqualTo(MapToken.Letter).Apply(Character.Upper).Repeat(3).Select(c => new string(c));

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
