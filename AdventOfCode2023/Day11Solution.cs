using System.Drawing;

namespace AdventOfCode2023;

public sealed class Day11Solution : ISolution
{
    public int Day =>
        11;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        Map.Parse(input).GetResult();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    internal sealed class Map
    {
        private readonly char[,] _data;

        private readonly Rectangle _bounds;

        private readonly byte[] _occupiedColumns;

        private readonly byte[] _occupiedRows;

        private readonly List<Point> _galaxies = new();

        private Map(char[,] data, Rectangle bounds)
        {
            _data = data;
            _bounds = bounds;

            _occupiedColumns = new byte[bounds.Width];
            _occupiedRows = new byte[bounds.Height];

            ScanCells(data, bounds.Size);
        }

        private IEnumerable<(int, int)> EnumerateGalaxyPairs()
        {
            for (var i = 0; i < _galaxies.Count - 1; i++)
            {
                for (int j = i + 1; j < _galaxies.Count; j++)
                {
                    yield return (i + 1, j + 1);
                }
            }
        }

        internal long GetDistance(int a, int b)
        {
            Point i = _galaxies[a - 1];
            Point j = _galaxies[b - 1];

            int dx = Math.Abs(i.X - j.X);
            int dy = Math.Abs(i.Y - j.Y);
            int sx = Math.Min(i.X, j.X);
            int sy = Math.Min(i.Y, j.Y);

            long distance = dx + dy;

            for (var x = 1; x < dx; x++)
            {
                if (_occupiedColumns[x + sx] == 0) distance++;
            }

            for (var y = 1; y < dy; y++)
            {
                if (_occupiedRows[y + sy] == 0) distance++;
            }

            return distance;
        }

        internal IEnumerable<long> EnumerateDistances() =>
            EnumerateGalaxyPairs().Select(p => GetDistance(p.Item1, p.Item2));

        internal long GetResult() =>
            EnumerateDistances().Aggregate(0L, (r, d) => r + d);

        private void ScanCells(char[,] data, Size size)
        {
            for (var y = 0; y < size.Height; y++)
            {
                for (var x = 0; x < size.Width; x++)
                {
                    if (data[y, x] != '#') continue;

                    _occupiedColumns[x] = 1;
                    _occupiedRows[y] = 1;
                    _galaxies.Add(new Point(x, y));
                }
            }
        }

        internal static Map Parse(IReadOnlyList<string> lines)
        {
            var size = new Size(lines[0].Length, lines.Count);
            var data = new char[size.Height, size.Width];

            for (var y = 0; y < size.Height; y++)
            {
                string line = lines[y];

                for (var x = 0; x < size.Width; x++)
                {
                    data[y, x] = line[x];
                }
            }

            return new Map(data, new Rectangle(Point.Empty, size));
        }
    }
}
