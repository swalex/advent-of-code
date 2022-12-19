namespace AdventOfCode.Solutions;

internal sealed class Day08Solution : SolutionBase
{
    private static readonly string[] DummyInput =
    {
        "11111",
        "12221",
        "12321",
        "12221",
        "11110"
    };
    
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        Solve(BuildMap(input));

    private static SolutionResult Solve(HeightMap heightMap)
    {
        // heightMap.Export("trees.txt");
        VisibilityMap visibilityMap = VisibilityMap.FromHeightMap(heightMap);
        // visibilityMap.Export("view.txt");

        var visible1 = 0;
        
        for (var y = 0; y < heightMap.Height; ++y)
        {
            for (var x = 0; x < heightMap.Width; ++x)
            {
                if (visibilityMap.IsVisible(x, y, heightMap[x, y])) ++visible1;
            }
        }

        return BuildResult(visible1, visible1);
    }

    private static (string, string) BuildResult(int visible1, int visible2) =>
        ($"Trees visible from the outside: {visible1}",
            $"Bar {visible2}");

    private static HeightMap BuildMap(IReadOnlyList<string> input)
    {
        var map = new HeightMap(input[0].Length, input.Count);
        var y = 0;
        foreach (string line in input)
        {
            var x = 0;
            foreach (char item in line)
            {
                map.SetHeight(x++, y, (sbyte)(item - '0'));
            }

            ++y;
        }

        return map;
    }

    private sealed class VisibilityMap
    {
        private readonly HeightMap _east;
        
        private readonly HeightMap _north;

        private readonly HeightMap _south;

        private readonly HeightMap _west;

        private readonly HeightMap _merged;
        
        private VisibilityMap(int width, int height)
        {
            _east = new HeightMap(width, height);
            _north = new HeightMap(width, height);
            _south = new HeightMap(width, height);
            _west = new HeightMap(width, height);
            _merged = new HeightMap(width, height);
        }

        internal static VisibilityMap FromHeightMap(HeightMap map)
        {
            var visibility = new VisibilityMap(map.Width, map.Height);

            for (var ny = 0; ny < map.Height; ++ny)
            {
                int sy = map.Height - ny - 1;
                
                for (var wx = 0; wx < map.Width; ++wx)
                {
                    int ex = map.Width - wx - 1;
                    
                    visibility._west.SetHeight(wx, ny, Math.Max(visibility._west[wx - 1, ny], map[wx, ny]));
                    visibility._east.SetHeight(ex, ny, Math.Max(visibility._east[ex + 1, ny], map[ex, ny]));
                    visibility._north.SetHeight(wx, ny, Math.Max(visibility._north[wx, ny - 1], map[wx, ny]));
                    visibility._south.SetHeight(wx, sy, Math.Max(visibility._south[wx, sy + 1], map[wx, sy]));
                }
            }

            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    visibility._merged.SetHeight(x, y,
                        Math.Min(
                            Math.Min(Math.Min(visibility._east[x, y], visibility._west[x, y]), visibility._south[x, y]),
                            visibility._north[x, y]));
                }
            }

            return visibility;
        }

        internal bool IsVisible(int x, int y, sbyte height) =>
            _west[x - 1, y] < height ||
            _east[x + 1, y] < height ||
            _north[x, y - 1] < height ||
            _south[x, y + 1] < height;

        internal void Export(string path)
        {
            _west.Export("west_" + path);
            _east.Export("east_" + path);
            _north.Export("north_" + path);
            _south.Export("south_" + path);
        }
    }
    
    private sealed class HeightMap
    {
        private const sbyte Savanna = -1;
        
        private readonly sbyte[,] _values;

        internal HeightMap(int width, int height)
        {
            _values = new sbyte[Width = width, Height = height];
        }
        
        internal int Height { get; }
        
        internal int Width { get; }

        internal sbyte this[int x, int y] =>
            x >= 0 && x < Width && y >= 0 && y < Height ? _values[x, y] : Savanna;

        internal void Export(string path)
        {
            using var writer = new StreamWriter(path);
            
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    writer.Write(GetGlyph(this[x, y]));
                }
                
                writer.WriteLine();
            }
        }

        internal void SetHeight(int x, int y, sbyte height) =>
            _values[x, y] = height;
        
        private static char GetGlyph(sbyte value) =>
            value switch
            {
                -1 => 'X',
                0 => '▁',
                1 => '▂',
                2 => '▃',
                3 => '▄',
                4 => '▅',
                5 => '▆',
                6 => '▇',
                7 => '▓',
                8 => '▒',
                9 => '░',
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
    }
}
