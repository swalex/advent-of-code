namespace AdventOfCode2023;

public sealed class Day14Solution : ISolution
{
    public int Day =>
        14;

    public long SolveFirstPuzzle(IReadOnlyList<string> input)
    {
        char[,] map = BuildMap(input);
        TiltNorth(map);

        DumpMap(map, ConsoleOutput.Instance);

        return CalculateLoad(input, map);
    }

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        SolveSecondPuzzle(input, ConsoleOutput.Instance);

    internal long SolveSecondPuzzle(IReadOnlyList<string> input, IOutput output)
    {
        char[,] map = BuildMap(input);

        IDirection[] directions =
        {
            NorthDirection.Instance,
            WestDirection.Instance,
            SouthDirection.Instance,
            EastDirection.Instance
        };

        for (var i = 0; i < 1000000000; i++)
        {
            Tilt(map, directions[i % 4]);
            if (i % 1000000 == 0) output.WriteLine($"{i / 1000000}%");
        }

        output.WriteLine();

        DumpMap(map, output);

        return CalculateLoad(input, map);
    }

    private static long CalculateLoad(IReadOnlyList<string> input, char[,] map)
    {
        long load = 0;
        for (var x = 0; x < input[0].Length; x++)
        {
            for (var y = 0; y < input.Count; y++)
            {
                if (map[x, y] == 'O')
                {
                    load += input.Count - y;
                }
            }
        }

        return load;
    }

    internal static char[,] BuildMap(IReadOnlyList<string> input)
    {
        var map = new char[input[0].Length, input.Count];

        for (var x = 0; x < input[0].Length; x++)
        {
            for (var y = 0; y < input.Count; y++)
            {
                map[x, y] = input[y][x];
            }
        }

        return map;
    }

    internal static void DumpMap(char[,] map, IOutput output)
    {
        for (var y = 0; y < map.GetLength(1); y++)
        {
            for (var x = 0; x < map.GetLength(0); x++)
            {
                output.Write(map[x, y]);
            }

            output.WriteLine();
        }
    }

    internal static void TiltNorth(char[,] map)
    {
        for (var x = 0; x < map.GetLength(0); x++)
        {
            var space = new FreeSpace(0);

            for (var y = 0; y < map.GetLength(1); y++)
            {
                char value = map[x, y];
                if (space.ShouldSwap(value, y, out int up)) (map[x, up], map[x, y]) = (map[x, y], map[x, up]);
            }
        }
    }

    internal static void Tilt(char[,] map, IDirection direction)
    {
        Parallel.For(0, direction.GetWidth(map), new ParallelOptions { MaxDegreeOfParallelism = 32 }, x =>
        {
            var space = new FreeSpace(0);

            for (var y = 0; y < direction.GetHeight(map); y++)
            {
                char value = direction.GetValue(map, x, y);
                if (space.ShouldSwap(value, y, out int up)) direction.Swap(map, x, y, up);
            }
        });
    }

    internal sealed class NorthDirection : IDirection
    {
        private NorthDirection()
        {
        }

        internal static IDirection Instance { get; } = new NorthDirection();

        public int GetWidth(char[,] map) =>
            map.GetLength(0);

        public int GetHeight(char[,] map) =>
            map.GetLength(1);

        public char GetValue(char[,] map, int x, int y) =>
            map[x, y];

        public void Swap(char[,] map, int x, int from, int to) =>
            (map[x, to], map[x, from]) = (map[x, from], map[x, to]);
    }

    internal sealed class SouthDirection : IDirection
    {
        private SouthDirection()
        {
        }

        internal static IDirection Instance { get; } = new SouthDirection();

        public int GetWidth(char[,] map) =>
            map.GetLength(0);

        public int GetHeight(char[,] map) =>
            map.GetLength(1);

        public char GetValue(char[,] map, int x, int y) =>
            map[x, GetHeight(map) - y - 1];

        public void Swap(char[,] map, int x, int from, int to)
        {
            int height = GetHeight(map) - 1;
            to = height - to;
            from = height - from;
            (map[x, to], map[x, from]) = (map[x, from], map[x, to]);
        }
    }

    internal sealed class WestDirection : IDirection
    {
        private WestDirection()
        {
        }

        internal static IDirection Instance { get; } = new WestDirection();

        public int GetWidth(char[,] map) =>
            map.GetLength(1);

        public int GetHeight(char[,] map) =>
            map.GetLength(0);

        public char GetValue(char[,] map, int x, int y) =>
            map[y, x];

        public void Swap(char[,] map, int x, int from, int to) =>
            (map[to, x], map[from, x]) = (map[from, x], map[to, x]);
    }

    internal sealed class EastDirection : IDirection
    {
        private EastDirection()
        {
        }

        internal static IDirection Instance { get; } = new EastDirection();

        public int GetWidth(char[,] map) =>
            map.GetLength(1);

        public int GetHeight(char[,] map) =>
            map.GetLength(0);

        public char GetValue(char[,] map, int x, int y) =>
            map[GetHeight(map) - y - 1, x];

        public void Swap(char[,] map, int x, int from, int to)
        {
            int height = GetHeight(map) - 1;
            to = height - to;
            from = height - from;
            (map[to, x], map[from, x]) = (map[from, x], map[to, x]);
        }
    }

    internal interface IDirection
    {
        int GetWidth(char[,] map);

        int GetHeight(char[,] map);

        char GetValue(char[,] map, int x, int y);

        void Swap(char[,] map, int x, int from, int to);
    }

    private sealed class FreeSpace
    {
        private int _start;

        private int _end;

        internal FreeSpace(int start)
        {
            _end = _start = start;
        }

        internal bool ShouldSwap(int value, int offset, out int free)
        {
            switch (value)
            {
            case '.':
                Increase();
                break;
            case '#':
                Reset(offset + 1);
                break;
            case 'O':
                if (TryUse(out free))
                {
                    Increase();
                    return true;
                }

                Reset(offset + 1);
                break;
            }

            free = 0;
            return false;
        }

        private void Increase() =>
            _end++;

        private void Reset(int offset) =>
            _start = _end = offset;

        private bool TryUse(out int offset)
        {
            offset = _start;
            if (_start == _end) return false;

            _start++;
            return true;
        }
    }
}
