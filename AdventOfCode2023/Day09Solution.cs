namespace AdventOfCode2023;

public sealed class Day09Solution : ISolution
{
    public int Day =>
        9;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        input.Select(ExtrapolateForward).Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        input.Select(ExtrapolateBackward).Sum();

    internal static long ExtrapolateForward(string line) =>
        Extrapolate(ParseLine(line), values => values.Last(), +1);

    internal static long ExtrapolateBackward(string line) =>
        Extrapolate(ParseLine(line), values => values.First(), -1);

    private static long Extrapolate(IReadOnlyCollection<long> values,
        Func<IReadOnlyCollection<long>, long> endSelector, long factor) =>
        endSelector.Invoke(values) + ComputeStep(values, endSelector, factor);

    private static long ComputeStep(IReadOnlyCollection<long> values, Func<IReadOnlyCollection<long>, long> endSelector, long factor)
    {
        List<long> differences = values.SkipLast(1).Zip(values.Skip(1), (a, b) => b - a).ToList();

        long endDifference = endSelector.Invoke(differences);
        long result = differences.GroupBy(d => d).Count() == 1
            ? endDifference
            : endDifference + ComputeStep(differences, endSelector, factor);
        return result * factor;
    }

    private static long[] ParseLine(string line) =>
        line.Split(' ').Select(long.Parse).ToArray();
}
