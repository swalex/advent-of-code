using System.Drawing;
using System.Numerics;
using System.Text;

namespace AdventOfCode2023;

public sealed class Day10Solution : ISolution
{
    public int Day =>
        10;

    public long SolveFirstPuzzle(IReadOnlyList<string> input)
    {
        Map map = Map.Parse(input);
        long result = (map.FindLength() + 1) / 2;

        return result;
    }

    public long SolveSecondPuzzle(IReadOnlyList<string> input)
    {
        Map map = Map.Parse(input);
        _ = (map.FindLength(out long result) + 1) / 2;
        map.Dump();

        return result;
    }

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

        internal int Height =>
            Bounds.Height;

        internal Size Size =>
            Bounds.Size;

        internal int Width =>
            Bounds.Width;

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

            return new Map(data, size, startPoint);
        }

        internal long FindLength() =>
            FindLength(out _);

        internal long FindLength(out long insideCount)
        {
            var startOffsets = new Size[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };

            foreach (Size startOffset in startOffsets)
            {
                if (TryFindLength(startOffset, out long length, out insideCount)) return length;
            }

            insideCount = 0;
            return 0;
        }

        internal void Dump()
        {
            var line = new StringBuilder();

            for (var y = 0; y < Height; y++)
            {
                line.Clear();

                for (var x = 0; x < Width; x++)
                {
                    line.Append(_visited[y, x] switch { 0 => '.', 1 => '+', 2 => 'L', 3 => 'R', _ => '?' });
                }

                Console.WriteLine(line);
            }
        }

        private bool TryFindLength(Size startOffset, out long length, out long insideCount)
        {
            ClearVisited();

            length = 0;
            insideCount = 0;

            var cwTurns = 0;
            var ccwTurns = 0;

            Point currentPosition = StartPoint;
            Point nextPosition = StartPoint + startOffset;

            while (IsValid(nextPosition) && !IsVisited(nextPosition))
            {
                char pipe = _data[nextPosition.Y, nextPosition.X];
                (Size ad, Size bd) = GetDirections(pipe);
                (Point a, Point b) = GetPositions(nextPosition, (ad, bd));

                if (a != currentPosition && b != currentPosition) return false;

                Point previousPosition = currentPosition;
                currentPosition = nextPosition;
                _visited[currentPosition.Y, currentPosition.X] = 1;

                Size d = (Size)nextPosition - (Size)previousPosition;
                Vector3 reference = new(d.Width, d.Height, 0);

                Point straight = currentPosition + ((Size)currentPosition - (Size)previousPosition);
                Vector3 straightVector = new(straight.X, straight.Y, 0);

                if (Vector3.Cross(reference, straightVector).Z > 0)
                {
                    cwTurns++;
                }
                else
                {
                    ccwTurns++;
                }

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        var offset = new Size(j, i);
                        Point p = currentPosition + offset;
                        if (!IsValid(p) || IsVisited(p)) continue;
                        if (p == previousPosition || p == nextPosition) continue;

                        d = (Size)p - (Size)previousPosition;
                        var v = new Vector3(d.Width, d.Height, 0);
                        _visited[p.Y, p.X] =
                            Vector3.Cross(reference, v).Z > 0 ? (byte)2 : (byte)3;
                    }
                }

                nextPosition = a == previousPosition ? b : a;
                length++;

                if (nextPosition != StartPoint) continue;
                insideCount = ComputeInsideCount(cwTurns > ccwTurns ? (byte)3 : (byte)2);

                return true;
            }

            return false;
        }

        private long ComputeInsideCount(byte innerSide)
        {
            FillMap(innerSide);
            return CountInsideFields(innerSide);
        }

        private void FillMap(byte innerSide)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var point = new Point(x, y);

                    if (_visited[y, x] == innerSide && HasEmptyNeighbors(point))
                    {
                        FillAt(point, innerSide);
                    }
                }
            }
        }

        private long CountInsideFields(byte innerSide)
        {
            long insideCount = 0;

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (_visited[y, x] == innerSide)
                    {
                        insideCount++;
                    }
                }
            }

            return insideCount;
        }

        private void FillAt(Point point, byte value)
        {
            var stack = new Stack<Point>();
            stack.Push(point);

            while (stack.TryPop(out point))
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        if (y == 0 && x == 0) continue;

                        var offset = new Size(x, y);
                        Point p = point + offset;
                        if (!IsValid(p) || _visited[p.Y, p.X] != 0) continue;

                        _visited[p.Y, p.X] = value;
                        stack.Push(p);
                    }
                }
            }
        }

        private bool HasEmptyNeighbors(Point point)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (y == 0 && x == 0) continue;

                    var offset = new Size(x, y);
                    Point p = point + offset;
                    if (IsValid(p) && _visited[p.Y, p.X] == 0) return true;
                }
            }

            return false;
        }

        private void ClearVisited()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _visited[y, x] = 0;
                }
            }
        }

        private static (Point, Point) GetPositions(Point position, (Size a, Size b) directions) =>
            (position + directions.a, position + directions.b);

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
            _visited[position.Y, position.X] == 1;

        private bool IsValid(Point position) =>
            Bounds.Contains(position);
    }
}
