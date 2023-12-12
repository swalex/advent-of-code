using System.Text;

namespace AdventOfCode2023;

public sealed class Day12Solution : ISolution
{
    public int Day =>
        12;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        input.Select(GetArrangementCount).Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        input.Select(GetUnfoldedArrangementCount).Sum();

    public static int GetArrangementCount(string line) =>
        GetArrangementCount(BuildRecord(line, 0));

    internal static long GetUnfoldedArrangementCount(string line)
    {
        long a = GetArrangementCount(BuildRecord(line, 1));
        long b = GetArrangementCount(BuildRecord(line, 2));
        long c = GetArrangementCount(BuildRecord(line, 0));
        long d = GetArrangementCount(BuildRecord(line, -2));

        if (a < b) (a, b) = (b, a);
        if (d != a * b) a = b;

        return Enumerable.Range(1, 3).Aggregate(a, (acc, _) => acc * a) * b;
    }

    private static int GetArrangementCount(Record record) =>
        record.EnumerateVariants().Count(record.Matches);

    internal static IEnumerable<string> EnumerateVariants(string line) =>
        BuildRecord(line, 0).EnumerateVariants();

    private static Record BuildRecord(string line, int flavor)
    {
        string[] parts = line.Split(' ', StringSplitOptions.TrimEntries);
        int[] counts = parts[1].Split(',').Select(int.Parse).ToArray();

        string pattern = parts[0];

        if (flavor < 0)
        {
            pattern = string.Join('?', Enumerable.Repeat(0, -flavor).Select(_ => pattern).ToList());
            counts = Enumerable.Repeat(0, -flavor).SelectMany(_ => counts).ToArray();
        }
        else
        {
            pattern = ApplyFlavor(pattern, flavor);
        }

        return new Record(pattern, counts);
    }

    private static string ApplyFlavor(string pattern, int flavor) =>
        flavor switch
        {
            0 => pattern,
            1 => $"?{pattern}",
            2 => $"{pattern}?",
            _ => throw new ArgumentOutOfRangeException(nameof(flavor), flavor, "Invalid flavor")
        };

    private sealed class Record(string pattern, IReadOnlyList<int> counts)
    {
        internal IEnumerable<string> EnumerateVariants() =>
            EnumerateVariants(pattern.Length, counts);

        private static IEnumerable<string> EnumerateVariants(int length, IReadOnlyList<int> workingCounts)
        {
            var builder = new StringBuilder();
            int failLength = workingCounts.Sum();
            int[] spaces = workingCounts
                .Select(_ => 1)
                .SkipLast(1)
                .ToArray();
            int room = length - failLength - spaces.Sum();
            var front = 0;

            while (front < room)
            {
                yield return BuildVariant(builder, length, workingCounts, front, spaces, out int remaining);

                if (remaining > 0)
                {
                    spaces[^1]++;
                }
                else
                {
                    for (int j = spaces.Length; j > 0; j--)
                    {
                        if (spaces[j - 1] == 1) continue;

                        if (j > 1) spaces[j - 2]++;
                        else front++;

                        spaces[j - 1] = 1;
                        break;
                    }
                }
            }

            yield return BuildVariant(builder, length, workingCounts, front, spaces, out int _);
        }

        private static string BuildVariant(StringBuilder builder, int length, IReadOnlyList<int> workingCounts,
            int front, IReadOnlyList<int> spaces, out int remaining)
        {
            builder.Clear();
            builder.Append('.', front);

            for (var i = 0; i < workingCounts.Count; i++)
            {
                builder.Append('#', workingCounts[i]);
                if (i < spaces.Count) builder.Append('.', spaces[i]);
            }

            remaining = length - builder.Length;
            builder.Append('.', remaining);
            return builder.ToString();
        }

        public bool Matches(string variant) =>
            !variant.Where(IsMatchingState).Any();

        private bool IsMatchingState(char variant, int index) =>
            variant != pattern[index] && pattern[index] != '?';
    }
}
