using System.Drawing;

namespace AdventOfCode2023;

public sealed class Day03Solution : ISolution
{
    public int Day =>
        3;

    public int SolveFirstPuzzle(IReadOnlyList<string> input)
    {
        Map map = GetMap(input);
        return map.EnumeratePartNumbers().Select(map.GetValue).Sum();
    }

    public int SolveSecondPuzzle(IReadOnlyList<string> input)
    {
        Map map = GetMap(input);
        List<Point> potentialGears = map.EnumeratePotentialGears().ToList();
        List<Map.Number> partNumbers = map.EnumeratePartNumbers().ToList();

        List<List<Map.Number>> gearSets = potentialGears
            .Select(gear => partNumbers.Where(number => number.IsAdjacentTo(gear)).ToList())
            .Where(set => set.Count == 2)
            .ToList();

        return gearSets.Select(set => set.Select(map.GetValue).Aggregate(1, (a, b) => a * b)).Sum();
    }

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

        internal IEnumerable<Point> EnumerateCells() =>
            Enumerable.Range(0, _height)
                .SelectMany(y => Enumerable.Range(0, _width).Select(x => new Point(x, y)));

        internal IEnumerable<Number> EnumeratePartNumbers() =>
            EnumerateNumbers().Where(IsPartNumber);

        internal IEnumerable<Point> EnumeratePotentialGears() =>
            EnumerateCells().Where(IsPotentialGear);

        private bool IsPotentialGear(Point position) =>
            GetValue(position) == '*';

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
                    else yield return new Number(left, y, x - left);
                }

                if (!inNumber) continue;

                yield return new Number(left, y, _width - left);
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

        internal bool IsInRange(Point point) =>
            point.X >= 0 && point.X < _width && point.Y >= 0 && point.Y < _height;

        internal IEnumerable<Point> EnumerateCells(Number number) =>
            number.EnumerateCells().Where(number.IsOutside).Where(IsInRange);

        internal bool IsPartNumber(Number number) =>
            EnumerateCells(number).Select(GetValue).Any(IsSymbol);

        private static bool IsSymbol(char value) =>
            !char.IsDigit(value) && value != '.';

        internal int GetValue(Number number) =>
            number
                .EnumerateCells()
                .Where(number.IsInside)
                .Select(GetValue)
                .Select(c => c - '0')
                .Aggregate(0, (a, b) => a * 10 + b);

        internal char GetValue(Point point) =>
            _map[point.Y, point.X];

        internal readonly record struct Number(int X, int Y, int Length)
        {
            internal IEnumerable<Point> EnumerateCells() =>
                EnumerateCells(X - 1, Y - 1, Length + 2, 3);

            internal bool IsAdjacentTo(Point point) =>
                EnumerateCells().Contains(point);

            internal bool IsInside(Point point) =>
                point.X >= X && point.X - X < Length && point.Y == Y;

            internal bool IsOutside(Point point) =>
                point.Y != Y || point.X < X || point.X - X >= Length;

            private static IEnumerable<Point> EnumerateCells(int x, int y, int width, int height) =>
                Enumerable
                    .Range(y, height)
                    .SelectMany(py => Enumerable.Range(x, width).Select(px => new Point(px, py)));
        }
    }
}
