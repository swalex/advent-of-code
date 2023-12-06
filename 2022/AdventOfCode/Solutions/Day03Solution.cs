namespace AdventOfCode.Solutions;

internal sealed class Day03Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        BuildResult(input.Select(FindDuplicateItem).Select(GetItemScore).Sum(),
            input.Chunk(3).Select(FindBadge).Select(GetItemScore).Sum());

    private static SolutionResult BuildResult(int score1, int score2) =>
        ($"Sum of Priorities of Duplicate Items: {score1}",
            $"Sum of Priorities of Group Badges: {score2}");

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
        item switch
        {
            >= 'a' and <= 'z' => item - 'a' + 1,
            >= 'A' and <= 'Z' => item - 'A' + 27,
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
        };
}
