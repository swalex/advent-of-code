using System.Text;

namespace AdventOfCode.Solutions;

internal sealed class Day14Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input)
    {
        List<Path> paths = input.Select(Path.Load).ToList();
        var source = new Position(500, 0);
        int restingUnitsWithoutFloor = Simulate(Simulation.CreateWithoutFloor(paths, source));
        int restingUnitsWithFloor = Simulate(Simulation.CreateWithFloor(paths, source));

        return ($"{restingUnitsWithoutFloor} units of sand came to rest without considering a floor.",
            $"{restingUnitsWithFloor} units of sand came to rest after considering a floor.");
    }

    private static int Simulate(Simulation simulation)
    {
        var restingUnits = 0;
        while (simulation.SpawnAndComeToRest()) ++restingUnits;

        Console.WriteLine(simulation);
        return restingUnits;
    }

    private sealed class Path
    {
        private readonly Position[] _points;

        internal Path(IEnumerable<Position> positions)
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

        internal static Simulation CreateWithoutFloor(IReadOnlyCollection<Path> paths, Position source) =>
            CreateSimulation(paths, source, ComputeBounds(paths, source));

        internal static Simulation CreateWithFloor(IReadOnlyCollection<Path> paths, Position source) =>
            CreateSimulationWithFloor(paths, source, ComputeBounds(paths, source));

        private static Simulation CreateSimulation(IEnumerable<Path> paths, Position source, Bounds bounds)
        {
            var simulation = new Simulation(bounds, source);
            simulation.Inflate();

            simulation.SetupRock(paths);
            return simulation;
        }

        private static Bounds ComputeBounds(IEnumerable<Path> paths, Position source) =>
            paths.Aggregate(new Bounds(source), (current, path) => path.Extend(current));

        private static Simulation CreateSimulationWithFloor(IEnumerable<Path> paths, Position source,
            Bounds bounds) =>
            CreateSimulation(AddFloor(paths, source, ref bounds), source, bounds);

        private static IEnumerable<Path> AddFloor(IEnumerable<Path> paths, Position source, ref Bounds bounds)
        {
            List<Path> pathList = paths.ToList();
            int y = bounds.Bottom + 2;
            int halfWidth = bounds.Height + 2;
            int l = source.X - halfWidth;
            int r = source.X + halfWidth;
            var floor = new Path(new[] { new Position(l, y), new Position(r, y) });
            bounds = floor.Extend(bounds);
            pathList.Add(floor);
            return pathList;
        }

        internal bool SpawnAndComeToRest()
        {
            if (!IsCell(_source, Source)) return false;
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

                if (!IsCell(newPosition, Air)) continue;
                
                descendPosition = newPosition;
                return true;
            }

            return false;
        }

        private bool IsCell(Position position, char type) =>
            _grid[position.X, position.Y] == type;

        private bool IsVoid(Position position) =>
            position.X < 0 || position.X >= _size.Width ||
            position.Y < 0 || position.Y >= _size.Height;
    }
}
