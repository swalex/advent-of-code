namespace Day03;

internal static class Program
{
    private static void Main()
    {
        string[] lines = File.ReadAllLines("input.txt");

        int score1 = lines.Select(FindDuplicateItem).Select(GetItemScore).Sum();
        int score2 = lines.Chunk(3).Select(FindBadge).Select(GetItemScore).Sum();

        Console.WriteLine($"1/2 Sum of Priorities of Duplicate Items: {score1}");
        Console.WriteLine($"2/2 Sum of Priorities of Group Badges: {score2}");
    }

    private static char FindDuplicateItem(string line) =>
        SplitAndFindDuplicateItem(line, line.Length / 2);

    private static char SplitAndFindDuplicateItem(string line, int halfLength) =>
        FindDuplicateItem(line[..halfLength], line[halfLength..]);

    private static char FindDuplicateItem(string firstCompartment, string secondCompartment) =>
        secondCompartment.Where(AsHashSet(firstCompartment).Contains).Distinct().Single();

    private static char FindBadge(IEnumerable<string> rucksacks) =>
        FindBadge(rucksacks.Select(AsHashSet).ToList());

    private static char FindBadge(IEnumerable<HashSet<char>> rucksacks) =>
        rucksacks.Aggregate((a, b) => new HashSet<char>(a.Intersect(b))).Single();

    private static HashSet<char> AsHashSet(string rucksack) =>
        new(rucksack);

    private static int GetItemScore(char item) =>
        item is >= 'a' and <= 'z'
            ? item - 'a' + 1
            : item is >= 'A' and <= 'Z'
                ? item - 'A' + 27
                : throw new ArgumentOutOfRangeException(nameof(item), item, null);
}