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
        RopeMap map = RopeMap.FromBounds(bounds);

        foreach (Vector move in moves)
        {
            map.ProcessMove(move);
        }

        return BuildResult(map.Visits, 0);
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

    private static SolutionResult BuildResult(int tailPositions, int bar) =>
        ($"The tail has visited {tailPositions} distinct positions.", "Bar");

    private sealed class RopeMap
    {
        private readonly bool[,] _visited;

        private Position _headPosition;

        private Position _tailPosition;

        private RopeMap(Size s, Position p)
        {
            _visited = new bool[s.Width + 1, s.Height + 1];
            _tailPosition = _headPosition = p;

            MarkVisited(p);
        }

        internal int Visits { get; private set; }

        internal static RopeMap FromBounds(Bounds b) =>
            new(b.Size, -b.TopLeft);

        internal void ProcessMove(Vector move) =>
            ProcessMove(move.Direction, move.Length);

        private void DragTail() =>
            DragTail(_headPosition - _tailPosition);

        private void DragTail(Vector delta)
        {
            if (delta.Length > 1) MarkVisited(_tailPosition += delta.Direction);
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
                _headPosition += direction;
                DragTail();
            }
        }
    }
}
