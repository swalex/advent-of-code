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

    public static IEnumerable<object[]> EnumerateExpectedSeeds() =>
        ExpectedSeeds.Select((seed, index) => new object[] { index, seed });

    [Theory]
    [MemberData(nameof(EnumerateExpectedSeeds))]
    public void VerifySeed(int index, int seed)
    {
        Day05Solution.Almanac almanac = Day05Solution.ParseAlmanac(ExampleData.Lines());

        Assert.Equal(seed, almanac.Seeds[index]);
    }
}
