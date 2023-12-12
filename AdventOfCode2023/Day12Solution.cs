using System.Text;

namespace AdventOfCode2023;

public sealed class Day12Solution : ISolution
{
    public int Day =>
        12;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        input.Select(GetArrangementCount).Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    public static int GetArrangementCount(string line) =>
        GetArrangementCount(BuildRecord(line));

    private static int GetArrangementCount(Record record) =>
        record.EnumerateVariants().Count(record.Matches);

    internal static IEnumerable<string> EnumerateVariants(string line) =>
        BuildRecord(line).EnumerateVariants();

    private static Record BuildRecord(string line)
    {
        string[] parts = line.Split(' ', StringSplitOptions.TrimEntries);
        int[] counts = parts[1].Split(',').Select(int.Parse).ToArray();

        return new Record(parts[0], counts);
    }

    private sealed class Record(string pattern, int[] counts)
    {
        internal IEnumerable<string> EnumerateVariants() =>
            EnumerateVariants(pattern.Length, counts);

        private static IEnumerable<string> EnumerateVariants(int length, int[] workingCounts)
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
