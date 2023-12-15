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
        throw new NotImplementedException();

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

    private sealed class FreeSpace
    {
        private int _start;

        private int _end;

        internal FreeSpace(int start)
        {
            _end = _start = start;
        }

        internal bool ShouldSwap(int value, int y, out int up)
        {
            switch (value)
            {
            case '.':
                Increase();
                break;
            case '#':
                Reset(y + 1);
                break;
            case 'O':
                if (TryUse(out up))
                {
                    Increase();
                    return true;
                }

                Reset(y + 1);
                break;
            }

            up = 0;
            return false;
        }

        private void Increase() =>
            _end++;

        private void Reset(int y) =>
            _start = _end = y;

        private bool TryUse(out int y)
        {
            y = _start;
            if (_start == _end) return false;

            _start++;
            return true;
        }
    }
}
