namespace AdventOfCode.Solutions;

internal sealed class Day09Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        Solve(input.Select(ParseMove).ToList());

    private static Bounds ComputeBounds(IEnumerable<Vector> moves)
    {
        var position = new Position();
        var bounds = new Bounds();

        foreach (Vector move in moves)
        {
            position += move;
            bounds += position;
        }

        return bounds;
    }

    private static SolutionResult Solve(List<Vector> moves)
    {
        Bounds bounds = ComputeBounds(moves);
        RopeMap map1 = RopeMap.FromBounds(bounds, 2);
        RopeMap map2 = RopeMap.FromBounds(bounds, 10);

        foreach (Vector move in moves)
        {
            map1.ProcessMove(move);
            map2.ProcessMove(move);
        }

        return BuildResult(map1.Visits, map2.Visits);
    }

    private static Vector ParseMove(string line) =>
        ParseMove(line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));

    private static Vector ParseMove(IReadOnlyList<string> parts) =>
        BuildMove(parts[0], int.Parse(parts[1]));

    private static Vector BuildMove(string direction, int distance) =>
        direction switch
        {
            "L" => Vector.Left(distance),
            "U" => Vector.Up(distance),
            "R" => Vector.Right(distance),
            "D" => Vector.Down(distance),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static SolutionResult BuildResult(int tail2Positions, int tail10Positions) =>
        ($"The tail of 2-knot rope has visited {tail2Positions} distinct positions.",
            $"The tail of 10-knot rope has visited {tail10Positions} distinct positions.");

    private sealed class RopeMap
    {
        private readonly Position[] _knots;
        
        private readonly bool[,] _visited;

        private RopeMap(Size s, Position p, int ropeLength)
        {
            _visited = new bool[s.Width, s.Height];
            _knots = Enumerable.Range(0, ropeLength).Select(_ => p).ToArray();

            MarkVisited(p);
        }

        internal int Visits { get; private set; }

        private Position HeadPosition
        {
            get => _knots[0];
            set => _knots[0] = value;
        }

        internal static RopeMap FromBounds(Bounds b, int ropeLength) =>
            new(b.Size, -b.TopLeft, ropeLength);

        internal void ProcessMove(Vector move) =>
            ProcessMove(move.Direction, move.Length);

        private void DragTail()
        {
            for (var i = 1; i < _knots.Length; i++)
            {
                DragTail(i, _knots[i - 1] - _knots[i]);
            }
        }

        private void DragTail(int knot, Vector delta)
        {
            if (delta.Length <= 1) return;

            _knots[knot] += delta.Direction;
            if (knot == _knots.Length - 1) MarkVisited(_knots[knot]);
        }

        private void MarkVisited(Position p)
        {
            if (_visited[p.X, p.Y]) return;
            
            _visited[p.X, p.Y] = true;
            Visits++;
        }

        private void ProcessMove(Vector direction, int length)
        {
            for (var i = 0; i < length; i++)
            {
                HeadPosition += direction;
                DragTail();
            }
        }
    }
}
