namespace AdventOfCode2023.Tests;

public sealed class TestDay05Solution
{
    private const string ExampleData =
        """
        seeds: 79 14 55 13

        seed-to-soil map:
        50 98 2
        52 50 48

        soil-to-fertilizer map:
        0 15 37
        37 52 2
        39 0 15

        fertilizer-to-water map:
        49 53 8
        0 11 42
        42 0 7
        57 7 4

        water-to-light map:
        88 18 7
        18 25 70

        light-to-temperature map:
        45 77 23
        81 45 19
        68 64 13

        temperature-to-humidity map:
        0 69 1
        1 0 69

        humidity-to-location map:
        60 56 37
        56 93 4
        """;

    private static readonly int[] ExpectedSeeds = { 79, 14, 55, 13 };

    private static readonly int[] ExpectedSoilNumbers = { 81, 14, 57, 13 };

    private static readonly int[] ExpectedLocations = { 82, 43, 86, 35 };

    private static int FirstExpectedResult =>
        ExpectedLocations.Min();

    public static IEnumerable<object[]> EnumerateExpectedSeeds() =>
        ExpectedSeeds.Select((seed, index) => new object[] { index, seed });

    public static IEnumerable<object[]> EnumerateExpectedSoilNumbers() =>
        ExpectedSoilNumbers.Select((number, index) => new object[] { index, number });

    public static IEnumerable<object[]> EnumerateExpectedLocations() =>
        ExpectedLocations.Select((location, index) => new object[] { index, location });

    [Theory]
    [MemberData(nameof(EnumerateExpectedSeeds))]
    public void VerifySeed(int index, int seed)
    {
        Day05Solution.Almanac almanac = Day05Solution.ParseAlmanac(ExampleData.Lines());

        Assert.Equal(seed, almanac.Seeds[index]);
    }

    [Theory]
    [MemberData(nameof(EnumerateExpectedSoilNumbers))]
    public void VerifySoilNumber(int index, int number)
    {
        Day05Solution.Almanac almanac = Day05Solution.ParseAlmanac(ExampleData.Lines());

        Assert.Equal(number, almanac.GetValueByIndex("soil", index));
    }

    [Theory]
    [MemberData(nameof(EnumerateExpectedLocations))]
    public void VerifyLocation(int index, int location)
    {
        Day05Solution.Almanac almanac = Day05Solution.ParseAlmanac(ExampleData.Lines());

        Assert.Equal(location, almanac.GetValueByIndex("location", index));
    }

    [Fact]
    public void VerifyFirstResult()
    {
        int expected = FirstExpectedResult;

        int actual = new Day05Solution().SolveFirstPuzzle(ExampleData.Lines());

        Assert.Equal(expected, actual);
    }
}
