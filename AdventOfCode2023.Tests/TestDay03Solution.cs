using System.Drawing;

namespace AdventOfCode2023.Tests;

public sealed class TestDay03Solution
{
    private const string ExampleData =
        """
        467..114..
        ...*......
        ..35..633.
        ......#...
        617*......
        .....+.58.
        ..592.....
        ......755.
        ...$.*....
        .664.598..
        """;

    public static IEnumerable<object[]> EnumerateCellPositionTestData() =>
        EnumerateCellTestData().Select(d => new object[] { d.Index, d.Position });

    public static IEnumerable<object[]> EnumerateCellValueTestData() =>
        EnumerateCellTestData().Select(d => new object[] { d.Index, d.Value });

    public static IEnumerable<object[]> EnumerateNumberValueTestData() =>
        new []{467,114,35,633,617,58,592,755,664,598}.Select((v, i) => new object[] { i, v });

    [Fact]
    public void Solution1()
    {
        const int expected = 4361;

        int actual = new Day03Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EnumerateNumbers()
    {
        const int expected = 10;

        List<Day03Solution.Map.Number> numbers = Day03Solution.GetMap(ExampleData.Lines()).EnumerateNumbers().ToList();

        Assert.Equal(expected, numbers.Count);
    }

    [Fact]
    public void EnumeratePartNumbers()
    {
        const int expected = 8;
        Day03Solution.Map map = Day03Solution.GetMap(ExampleData.Lines());

        List<Day03Solution.Map.Number> numbers = map
            .EnumerateNumbers()
            .Where(map.IsPartNumber)
            .ToList();

        Assert.Equal(expected, numbers.Count);
    }

    [Fact]
    public void FirstNumberIsPartNumber()
    {
        Day03Solution.Map map = Day03Solution.GetMap(ExampleData.Lines());
        List<Day03Solution.Map.Number> numbers = map.EnumerateNumbers().ToList();

        Assert.True(map.IsPartNumber(numbers[0]));
    }

    [Fact]
    public void EnumerateFirstNumberCells()
    {
        Day03Solution.Map map = Day03Solution.GetMap(ExampleData.Lines());
        List<Day03Solution.Map.Number> numbers = map.EnumerateNumbers().ToList();
        List<Point> cells = map.EnumerateCells(numbers[0]).ToList();

        Assert.Equal(5, cells.Count);
    }

    [Theory]
    [MemberData(nameof(EnumerateCellValueTestData))]
    public void FirstNumberCellValue(int index, char expectedValue)
    {
        Day03Solution.Map map = Day03Solution.GetMap(ExampleData.Lines());
        List<Day03Solution.Map.Number> numbers = map.EnumerateNumbers().ToList();
        List<char> cells = map.EnumerateCells(numbers[0]).Select(map.GetValue).ToList();

        Assert.Equal(expectedValue, cells[index]);
    }

    [Theory]
    [MemberData(nameof(EnumerateCellPositionTestData))]
    public void FirstNumberCellPosition(int index, Point expectedValue)
    {
        Day03Solution.Map map = Day03Solution.GetMap(ExampleData.Lines());
        List<Day03Solution.Map.Number> numbers = map.EnumerateNumbers().ToList();
        List<Point> cells = map.EnumerateCells(numbers[0]).ToList();

        Assert.Equal(expectedValue, cells[index]);
    }

    [Fact]
    public void GetMapValue()
    {
        Day03Solution.Map map = Day03Solution.GetMap(ExampleData.Lines());

        Assert.Equal('#', map.GetValue(new Point(6, 3)));
    }

    [Theory]
    [MemberData(nameof(EnumerateNumberValueTestData))]
    public void GetNumberValue(int index, int expectedValue)
    {
        Day03Solution.Map map = Day03Solution.GetMap(ExampleData.Lines());
        List<Day03Solution.Map.Number> numbers = map.EnumerateNumbers().ToList();

        Assert.Equal(expectedValue, map.GetValue(numbers[index]));
    }

    private static IEnumerable<(int Index, char Value, Point Position)> EnumerateCellTestData()
    {
        yield return (0, '.', new Point(3, 0));
        yield return (1, '.', new Point(0, 1));
        yield return (4, '*', new Point(3, 1));
    }
}
