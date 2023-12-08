namespace AdventOfCode2023;

public sealed class Day06Solution : ISolution
{
    public int Day =>
        6;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        EnumerateOptions(input).Aggregate(1, (a, b) => a * b);

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        EnumerateOptions(input.Select(ImproveKerning)).Aggregate(1, (a, b) => a * b);

    private static string ImproveKerning(string line) =>
        line.Replace(" ", string.Empty).Replace(":", ": ");

    public static IEnumerable<int> EnumerateOptions(IEnumerable<string> lines) =>
        ParseSetup(lines).EnumerateOptions().Select(o => o.Count());

    private static Setup ParseSetup(IEnumerable<string> lines) =>
        BuildSetup(SplitInput(lines).ToList());

    private static Setup BuildSetup(IReadOnlyList<IEnumerable<long>> splitInput) =>
        new(splitInput[0].ToArray(), splitInput[1].ToArray());

    private static IEnumerable<IEnumerable<long>> SplitInput(IEnumerable<string> lines) =>
        lines.Select(SplitLine);

    private static IEnumerable<long> SplitLine(string line) =>
        line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(long.Parse);

    internal sealed record Setup(long[] Times, long[] Distances)
    {
        internal IEnumerable<IEnumerable<long>> EnumerateOptions() =>
            Times.Select((time, index) =>
                EnumerateOptionsFromTime(time).Where(distance => distance > Distances[index]));

        private static IEnumerable<long> EnumerateOptionsFromTime(long time) =>
            Enumerate(time).Select(ComputeDistance(time));

        private static IEnumerable<long> Enumerate(long time)
        {
            for (long i = 0; i < time; i++) yield return i;
        }

        private static Func<long, long> ComputeDistance(long time) =>
            acceleration => (time - acceleration) * acceleration;
    }
}
