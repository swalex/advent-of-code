namespace AdventOfCode2023;

public sealed class Day09Solution : ISolution
{
    public int Day =>
        9;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        input.Select(Extrapolate).Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    internal static long Extrapolate(string line) =>
        Extrapolate(ParseLine(line));

    private static long Extrapolate(IReadOnlyCollection<long> values) =>
        values.Last() + ComputeNextStep(values);

    private static long ComputeNextStep(IReadOnlyCollection<long> values)
    {
        List<long> differences = values.SkipLast(1).Zip(values.Skip(1), (a, b) => b - a).ToList();

        long lastDifference = differences.Last();
        return differences.GroupBy(d => d).Count() == 1
            ? lastDifference
            : lastDifference + ComputeNextStep(differences);
    }

    private static long[] ParseLine(string line) =>
        line.Split(' ').Select(long.Parse).ToArray();
}
