using System.Drawing;

namespace AdventOfCode2023;

public sealed class Day10Solution : ISolution
{
    public int Day =>
        10;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        (Map.Parse(input).FindLength() + 1) / 2;

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    internal sealed class Map
    {
        private readonly char[,] _data;

        private readonly byte[,] _visited;

        private Map(char[,] data, Size size, Point startPoint)
        {
            _data = data;
            _visited = new byte[size.Height, size.Width];
            Bounds = new Rectangle(Point.Empty, size);
            StartPoint = startPoint;
        }

        internal Rectangle Bounds { get; }

        internal Size Size =>
            Bounds.Size;

        internal Point StartPoint { get; }

        internal static Map Parse(IReadOnlyList<string> input)
        {
            var size = new Size(input[0].Length, input.Count);
            var data = new char[size.Height, size.Width];

            Point startPoint = default;

            for (var y = 0; y < size.Height; y++)
            {
                string line = input[y];

                for (var x = 0; x < size.Width; x++)
                {
                    char value = line[x];
                    data[y, x] = value;

                    if (value == 'S') startPoint = new Point(x, y);
                }
            }

            var startOffsets = new[] { new Point(1, 0), new Point(0, 1), new Point(-1, 0), new Point(0, -1) };

            return new Map(data, size, startPoint);
        }

        internal long FindLength()
        {
            var startOffsets = new Size[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };

            foreach (Size startOffset in startOffsets)
            {
                if (TryFindLength(startOffset, out long length)) return length;
            }

            return 0;
        }

        private bool TryFindLength(Size offset, out long length)
        {
            for (var y = 0; y < Size.Height; y++)
            {
                for (var x = 0; x < Size.Width; x++)
                {
                    _visited[y, x] = 0;
                }
            }

            length = 0;

            Point currentPosition = StartPoint;
            Point nextPosition = StartPoint + offset;

            while (IsValid(nextPosition) && !IsVisited(nextPosition))
            {
                (Point a, Point b) = GetConnections(nextPosition);
                if (a != currentPosition && b != currentPosition) return false;

                Point previousPosition = currentPosition;
                currentPosition = nextPosition;
                _visited[currentPosition.Y, currentPosition.X] = 1;

                nextPosition = a == previousPosition ? b : a;
                length++;

                if (nextPosition == StartPoint) return true;
            }

            return false;
        }

        private (Point, Point) GetConnections(Point position)
        {
            (Size a, Size b) = GetDirections(_data[position.Y, position.X]);
            return (position + a, position + b);
        }

        private static (Size, Size) GetDirections(char value) =>
            value switch
            {
                'L' => (new Size(0, -1), new Size(1, 0)),
                'J' => (new Size(0, -1), new Size(-1, 0)),
                '7' => (new Size(0, 1), new Size(-1, 0)),
                'F' => (new Size(0, 1), new Size(1, 0)),
                '-' => (new Size(-1, 0), new Size(1, 0)),
                '|' => (new Size(0, -1), new Size(0, 1)),
                _ => (Size.Empty, Size.Empty)
            };

        private bool IsVisited(Point position) =>
            _visited[position.Y, position.X] > 0;

        private bool IsValid(Point position) =>
            Bounds.Contains(position);
    }
}
