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
        ViewDistanceMaps distanceMaps = ViewDistanceMaps.FromHeightMap(heightMap);
        // distanceMaps.Export("dist.txt");

        var visible = 0;
        
        for (var y = 0; y < heightMap.Height; ++y)
        {
            for (var x = 0; x < heightMap.Width; ++x)
            {
                if (visibilityMap.IsVisible(x, y, heightMap[x, y])) ++visible;
            }
        }

        var bestScore = 0;
        for (var y = 0; y < heightMap.Height; ++y)
        {
            for (var x = 0; x < heightMap.Width; ++x)
            {
                bestScore = Math.Max(distanceMaps.GetScore(x, y, heightMap[x, y]), bestScore);
            }
        }
        
        return BuildResult(visible, bestScore);
    }

    private static (string, string) BuildResult(int visible, int bestScenicScore) =>
        ($"Trees visible from the outside: {visible}",
            $"Highest possible scenic score: {bestScenicScore}");

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
        
        private VisibilityMap(int width, int height)
        {
            _east = new HeightMap(width, height);
            _north = new HeightMap(width, height);
            _south = new HeightMap(width, height);
            _west = new HeightMap(width, height);
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

            return visibility;
        }

        internal bool IsVisible(int x, int y, int height) =>
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

    private sealed class ViewDistanceMaps
    {
        private readonly ViewDistanceMap[] _maps = new ViewDistanceMap[10];
        
        private ViewDistanceMaps()
        {
        }

        internal static ViewDistanceMaps FromHeightMap(HeightMap heightMap)
        {
            var maps = new ViewDistanceMaps();
            for (var i = 0; i < maps._maps.Length; i++)
            {
                maps._maps[i] = ViewDistanceMap.FromHeightMap(heightMap, i);
            }

            return maps;
        }

        internal void Export(string path)
        {
            foreach (ViewDistanceMap map in _maps)
            {
                map.Export(path);
            }
        }

        internal int GetScore(int x, int y, int height) =>
            _maps[height].GetScore(x, y, height);
    }
    
    private sealed class ViewDistanceMap
    {
        private readonly int _targetHeight;
        
        private readonly HeightMap _east;
        
        private readonly HeightMap _north;

        private readonly HeightMap _south;

        private readonly HeightMap _west;
        
        private ViewDistanceMap(int width, int height, int targetHeight)
        {
            _targetHeight = targetHeight;
            _east = new HeightMap(width, height);
            _north = new HeightMap(width, height);
            _south = new HeightMap(width, height);
            _west = new HeightMap(width, height);
        }

        internal static ViewDistanceMap FromHeightMap(HeightMap map, int targetHeight)
        {
            var distance = new ViewDistanceMap(map.Width, map.Height, targetHeight);

            for (var ny = 0; ny < map.Height; ++ny)
            {
                int sy = map.Height - ny - 1;
                
                for (var wx = 0; wx < map.Width; ++wx)
                {
                    int ex = map.Width - wx - 1;

                    distance._west.SetHeight(wx, ny,
                        map[wx - 1, ny] < targetHeight ? distance._west[wx - 1, ny] + 1 : 1);

                    distance._east.SetHeight(ex, ny,
                        map[ex + 1, ny] < targetHeight ? distance._east[ex + 1, ny] + 1 : 1);

                    distance._north.SetHeight(wx, ny,
                        map[wx, ny - 1] < targetHeight ? distance._north[wx, ny - 1] + 1 : 1);

                    distance._south.SetHeight(wx, sy,
                        map[wx, sy + 1] < targetHeight ? distance._south[wx, sy + 1] + 1 : 1);
                }
            }

            return distance;
        }

        internal int GetScore(int x, int y, int height) =>
            height == _targetHeight ? _west[x, y] * _east[x, y] * _north[x, y] * _south[x, y] : 0;

        internal void Export(string path)
        {
            _west.Export($"west_{_targetHeight}_" + path);
            _east.Export($"east_{_targetHeight}_" + path);
            _north.Export($"north_{_targetHeight}_" + path);
            _south.Export($"south_{_targetHeight}_" + path);
        }
    }
    
    private sealed class HeightMap
    {
        private const int Savanna = -1;
        
        private readonly int[,] _values;

        internal HeightMap(int width, int height)
        {
            _values = new int[Width = width, Height = height];
        }
        
        internal int Height { get; }
        
        internal int Width { get; }

        internal int this[int x, int y] =>
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

        internal void SetHeight(int x, int y, int height) =>
            _values[x, y] = height;
        
        private static char GetGlyph(int value) =>
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
                _ => '▣'
            };
    }
}
