namespace AdventOfCode2023;

public sealed class Day06Solution : ISolution
{
    public int Day =>
        6;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        EnumerateOptions(input).Aggregate(1, (a, b) => a * b);

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    public static IEnumerable<int> EnumerateOptions(IEnumerable<string> lines) =>
        ParseSetup(lines).EnumerateOptions().Select(o => o.Count());

    private static Setup ParseSetup(IEnumerable<string> lines) =>
        BuildSetup(SplitInput(lines).ToList());

    private static Setup BuildSetup(IReadOnlyList<IEnumerable<int>> splitInput) =>
        new(splitInput[0].ToArray(), splitInput[1].ToArray());

    private static IEnumerable<IEnumerable<int>> SplitInput(IEnumerable<string> lines) =>
        lines.Select(SplitLine);

    private static IEnumerable<int> SplitLine(string line) =>
        line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(int.Parse);

    internal sealed record Setup(int[] Times, int[] Distances)
    {
        internal IEnumerable<IEnumerable<int>> EnumerateOptions() =>
            Times.Select((time, index) =>
                EnumerateOptionsFromTime(time).Where(distance => distance > Distances[index]));

        private static IEnumerable<int> EnumerateOptionsFromTime(int time) =>
            Enumerable
                .Range(0, time)
                .Select(ComputeDistance(time));

        private static Func<int, int> ComputeDistance(int time) =>
            acceleration => (time - acceleration) * acceleration;
    }
}
