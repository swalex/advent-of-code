namespace AdventOfCode2023;

public sealed class Day03Solution : ISolution
{
    public int Day =>
        3;

    public int SolveFirstPuzzle(IReadOnlyList<string> input)
    {
        Map map = GetMap(input);
        throw new NotImplementedException();
    }

    public int SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    internal static Map GetMap(IReadOnlyList<string> input) =>
        Map.FromLines(input);

    internal sealed class Map
    {
        private readonly int _height;

        private readonly char[,] _map;

        private readonly int _width;

        private Map(char[,] map)
        {
            _map = map;
            _height = map.GetLength(0);
            _width = map.GetLength(1);
        }

        internal IEnumerable<Number> EnumerateNumbers()
        {
            var inNumber = false;

            for (var y = 0; y < _height; y++)
            {
                var left = 0;

                for (var x = 0; x < _width; x++)
                {
                    bool isDigit = char.IsDigit(_map[y, x]);
                    if (isDigit == inNumber) continue;

                    inNumber = isDigit;
                    if (isDigit) left = x;
                    else yield return new Number(left, y, x - left, 0);
                }

                if (!inNumber) continue;

                yield return new Number(left, y, _width - left, 0);
                inNumber = false;
            }
        }

        internal static Map FromLines(IReadOnlyList<string> lines)
        {
            var map = new char[lines.Count, lines[0].Length];
            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    map[y, x] = lines[y][x];
                }
            }

            return new Map(map);
        }

        internal readonly record struct Number(int X, int Y, int Length, int Value);
    }
}
