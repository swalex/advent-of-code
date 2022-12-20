using System.Text;

namespace AdventOfCode.Solutions;

internal sealed class Day14Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input)
    {
        List<Path> paths = input.Select(Path.Load).ToList();
        var simulation = Simulation.Create(paths, new Position(500, 0));

        var restingUnits = 0;
        while (simulation.SpawnAndComeToRest()) ++restingUnits;

        Console.WriteLine(simulation);

        return ($"{restingUnits} units of sand came to rest.", "Bar");
    }

    private sealed class Path
    {
        private readonly Position[] _points;

        private Path(IEnumerable<Position> positions)
        {
            _points = positions.ToArray();
        }

        internal IEnumerable<Position> EnumeratePositions()
        {
            Position current = _points[0];
            foreach (Position next in _points)
            {
                Vector delta = next - current;
                Vector direction = delta.Direction;
                
                do
                {
                    yield return current;
                    current += direction;
                } while (current != next);
            }

            yield return current;
        }

        internal Bounds Extend(Bounds bounds) =>
            _points.Aggregate(bounds, (current, point) => current + point);

        internal static Path Load(string line) =>
            Load(line.Split(" -> "));

        private static Path Load(IEnumerable<string> split) =>
            new(split.Select(LoadPosition));

        private static Position LoadPosition(string input) =>
            LoadPosition(input.Split(','));

        private static Position LoadPosition(IReadOnlyList<string> parts) =>
            new(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    private sealed class Simulation
    {
        private const char Air = '.';

        private const char Rock = '#';

        private const char Sand = 'o';

        private const char Source = '+';

        private const char Void = ' ';

        private static readonly Vector Down = new Vector(0, 1);

        private static readonly Vector DownLeft = new Vector(-1, 1);

        private static readonly Vector DownRight = new Vector(1, 1);

        private static readonly Vector[] DescendDirections = { Down, DownLeft, DownRight };
        
        private readonly char[,] _grid;

        private readonly Vector _offset;
        
        private readonly Size _size;

        private readonly Position _source;
        
        private Simulation(Bounds bounds, Position source)
        {
            _offset = Position.Zero - bounds.TopLeft;
            _source = source + _offset;
            _size = bounds.Size;
            _grid = new char[_size.Width, _size.Height];
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var y = 0; y < _size.Height; ++y)
            {
                for (var x = 0; x < _size.Width; ++x)
                {
                    builder.Append(_grid[x, y]);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        internal static Simulation Create(IReadOnlyCollection<Path> paths, Position source)
        {
            Bounds bounds = paths.Aggregate(new Bounds(source), (current, path) => path.Extend(current));
            var simulation = new Simulation(bounds, source);
            simulation.Inflate();

            simulation.SetupRock(paths);
            return simulation;
        }

        internal bool SpawnAndComeToRest()
        {
            Position position = _source;

            while (TryGetDescendPosition(position, out position))
            {
                if (IsVoid(position)) return false;
            }

            SetCell(position, Sand);
            return true;
        }

        private void Inflate()
        {
            for (var y = 0; y < _size.Height; ++y)
            {
                for (var x = 0; x < _size.Width; ++x)
                {
                    _grid[x, y] = Air;
                }
            }

            SetCell(_source, Source);
        }

        private static bool IsAir(char cell) =>
            cell == '.';

        private void SetupRock(IEnumerable<Path> paths)
        {
            foreach (Path path in paths) SetupRock(path);
        }

        private void SetupRock(Path path)
        {
            foreach (Position position in path.EnumeratePositions()) SetCell(position + _offset, Rock);
        }

        private void SetCell(Position position, char value) =>
            _grid[position.X, position.Y] = value;

        private bool TryGetDescendPosition(Position position, out Position descendPosition)
        {
            descendPosition = position;
            foreach (Vector descendDirection in DescendDirections)
            {
                Position newPosition = position + descendDirection;
                if (IsVoid(newPosition))
                {
                    descendPosition = newPosition;
                    return true;
                }

                if (!IsAir(newPosition)) continue;
                
                descendPosition = newPosition;
                return true;
            }

            return false;
        }

        private bool IsAir(Position position) =>
            IsAir(_grid[position.X, position.Y]);

        private bool IsVoid(Position position) =>
            position.X < 0 || position.X >= _size.Width ||
            position.Y < 0 || position.Y >= _size.Height;
    }
}
