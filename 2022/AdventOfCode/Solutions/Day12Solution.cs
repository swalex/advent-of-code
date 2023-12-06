using System.Collections.Immutable;
using System.Text;

namespace AdventOfCode.Solutions;

internal sealed class Day12Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input)
    {
        HeightMap map = HeightMap.FromInput(input);
        Console.WriteLine(map);
        Console.WriteLine(map.FindStart());
        Console.WriteLine(map.FindEnd());
        List<Position> path = FindPath(map).ToList();
        Console.WriteLine(map.ToString(path));

        int shortestPath = map.FindLowlands()
            .Select(p => FindPath(map, p).Count())
            .Where(l => l > 0)
            .MinBy(l => l);

        return BuildResult(path.Count - 1, shortestPath - 1);
    }

    private static SolutionResult BuildResult(int ownPathLength, int shortestPathLength) =>
        ($"Shortest Own Path Length: {ownPathLength}",
            $"Shortest Hiking Trail Length: {shortestPathLength}");

    private static IEnumerable<Position> ReconstructPath(IDictionary<Position, Position> cameFrom, Position current)
    {
        var path = new Stack<Position>();

        do
        {
            path.Push(current);
        } while (cameFrom.TryGetValue(current, out current));

        return path;
    }

    private static IEnumerable<Position> FindPath(HeightMap map) =>
        FindPath(map, map.FindStart());

    private static IEnumerable<Position> FindPath(HeightMap map, Position start)
    {
        Vector[] directions = { Vector.Right(1), Vector.Down(1), Vector.Left(1), Vector.Up(1) };
        
        Position end = map.FindEnd();
        
        var openSet = new HashSet<Position> { start };
        var cameFrom = new Dictionary<Position, Position>();
        var gScore = new ScoreMap(start, 0);
        var fScore = new ScoreMap(start, EstimateCost(start, end));

        while (openSet.Any())
        {
            Position current = fScore.LowestIn(openSet);

            if (current == end) return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (Vector direction in directions)
            {
                Position neighbor = current + direction;
                float tentativeGScore = gScore[current] + map.GetEffort(current, neighbor);
                if (tentativeGScore >= gScore[neighbor]) continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = tentativeGScore + EstimateCost(neighbor, end);
                openSet.Add(neighbor);
            }
        }

        return ImmutableArray<Position>.Empty;
    }

    private static int EstimateCost(Position from, Position to) =>
        (to - from).CoarseLength;

    private sealed class ScoreMap
    {
        private readonly Dictionary<Position, float> _values = new();

        internal ScoreMap(Position start, float score)
        {
            _values[start] = score;
        }

        internal Position LowestIn(IReadOnlySet<Position> set) =>
            _values
                .Where(p => set.Contains(p.Key))
                .MinBy(p => p.Value).Key;

        public float this[Position index]
        {
            get => _values.TryGetValue(index, out float result) ? result : _values[index] = float.PositiveInfinity;
            set => _values[index] = value;
        }
    }

    private sealed class HeightMap
    {
        private readonly char[,] _cells;

        private readonly Size _size;

        private HeightMap(Size size)
        {
            _size = size;
            _cells = new char[size.Width, size.Height];
        }
        
        public override string ToString() =>
            ToString(ArraySegment<Position>.Empty);

        internal Position FindEnd() =>
            Find(c => c == 'E');

        internal Position FindStart() =>
            Find(c => c == 'S');

        internal IEnumerable<Position> FindLowlands()
        {
            for (var y = 0; y < _size.Height; ++y)
            {
                for (var x = 0; x < _size.Width; ++x)
                {
                    if (_cells[x, y] is 'a' or 'S') yield return new Position(x, y);
                }
            }
        }

        internal static HeightMap FromInput(IReadOnlyCollection<string> input)
        {
            var map = new HeightMap(new Size(input.First().Length, input.Count));

            var y = 0;
            foreach (string line in input)
            {
                var x = 0;
                foreach (char cell in line)
                {
                    map._cells[x++, y] = cell;
                }

                ++y;
            }

            return map;
        }

        internal float GetEffort(Position current, Position neighbor) =>
            GetEffort(GetCell(current), GetCell(neighbor));

        private static float GetEffort(char current, char neighbor) =>
            CanPass(current, neighbor) ? 1 : float.PositiveInfinity;

        private static bool CanPass(char current, char neighbor)
        {
            if (current == 'S') return neighbor == 'a';
            if (neighbor == 'E') return current == 'z';
            
            return neighbor <= current + 1;
        }

        internal string ToString(IEnumerable<Position> path)
        {
            List<Position> positionList = path.ToList();
            
            var builder = new StringBuilder();
            for (var y = 0; y < _size.Height; ++y)
            {
                if (y > 0) builder.AppendLine();
                
                for (var x = 0; x < _size.Width; ++x)
                {
                    builder.Append(GetPathOrCell(positionList, new Position(x, y)));
                }
            }

            return builder.ToString();
        }

        private char GetPathOrCell(IList<Position> path, Position position) =>
            path.Contains(position) ? GetDirectedPath(path, position) : GetCell(position);

        private static char GetDirectedPath(IList<Position> path, Position position) =>
            GetDirectedPath(path, position, path.IndexOf(position));

        private static char GetDirectedPath(IList<Position> path, Position position, int index) =>
            index == path.Count - 1 ? '+' : GetDirection(path[index + 1] - position);

        private static char GetDirection(Vector position) =>
            position switch
            {
                (1, 0) => '→',
                (0, 1) => '↓',
                (-1, 0) => '←',
                (0, -1) => '↑',
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
            };

        private char GetCell(Position position) =>
            IsValid(position) ? _cells[position.X, position.Y] : '~';

        private bool IsValid(Position position) =>
            position.X >= 0 && position.X < _size.Width &&
            position.Y >= 0 && position.Y < _size.Height;

        private Position Find(Func<char, bool> predicate)
        {
            for (var y = 0; y < _size.Height; ++y)
            {
                for (var x = 0; x < _size.Width; ++x)
                {
                    if (predicate.Invoke(_cells[x, y])) return new Position(x, y);
                }
            }

            throw new InvalidOperationException();
        }
    }
}
