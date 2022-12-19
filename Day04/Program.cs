namespace Day04;

internal static class Program
{
    private static void Main()
    {
        List<(SectionRange, SectionRange)> ranges = File.ReadAllLines("input.txt").Select(ConvertToRanges).ToList();
        int count1 = ranges.Count(OneFullyContainsOther);
        int count2 = ranges.Count(RangesOverlap);

        Console.WriteLine($"1/2 Number of Fully Contained Ranges: {count1}");
        Console.WriteLine($"2/2 Number of Overlapping Ranges: {count2}");
    }

    private static bool OneFullyContainsOther((SectionRange A, SectionRange B) value) =>
        OneFullyContainsOther(value.A, value.B);

    private static bool OneFullyContainsOther(SectionRange a, SectionRange b) =>
        FirstFullyContainsSecond(a, b) || FirstFullyContainsSecond(b, a);

    private static bool FirstFullyContainsSecond(SectionRange a, SectionRange b) =>
        a.Start <= b.Start && a.End >= b.End;

    private static bool RangesOverlap((SectionRange A, SectionRange B) value) =>
        RangesOverlap(value.A, value.B);

    private static bool RangesOverlap(SectionRange a, SectionRange b) =>
        EndOverlapsWithStart(a, b) || EndOverlapsWithStart(b, a);

    private static bool EndOverlapsWithStart(SectionRange a, SectionRange b) =>
        a.Start <= b.Start && a.End >= b.Start;

    private static (SectionRange, SectionRange) ConvertToRanges(string line) =>
        ConvertToRanges(line.Split(','));

    private static (SectionRange, SectionRange) ConvertToRanges(IReadOnlyList<string> parts) =>
        (ConvertToRange(parts[0]), ConvertToRange(parts[1]));

    private static SectionRange ConvertToRange(string part) =>
        ConvertToRange(part.Split('-'));

    private static SectionRange ConvertToRange(IReadOnlyList<string> sides) =>
        new(int.Parse(sides[0]), int.Parse(sides[1]));

    private readonly record struct SectionRange(int Start, int End);
}