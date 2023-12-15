namespace AdventOfCode2023;

public sealed class Day15Solution : ISolution
{
    public int Day =>
        15;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        input.Select(SolveFirstPuzzleOnLine).Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        throw new NotImplementedException();

    private static long SolveFirstPuzzleOnLine(string line) =>
        line.Split(',').Select(ComputeHash).Sum();

    private static long ComputeHash(string token) =>
        FancyHash.Calculate(token);

    internal static class FancyHash
    {
        internal static int Calculate(string token) =>
            token.Aggregate(0, Add);

        private static int Add(int value, char letter) =>
            (value + letter) * 17 % 256;
    }

    internal sealed class Lens
    {
        private Lens(string label, int focalLength)
        {
            Label = label;
            FocalLength = focalLength;
            Box = label.FancyHash();
        }

        internal int FocalLength { get; }

        internal int Box { get; }

        internal string Label { get; }

        internal static Lens? DecodeLens(string code) =>
            code.Contains('=') ? DecodeLens(code.Split('=')) : null;

        private static Lens DecodeLens(IReadOnlyList<string> parts) =>
            new(parts[0], int.Parse(parts[1]));
    }
}

internal static class ExtensionsForFancyHash
{
    internal static int FancyHash(this string token) =>
        Day15Solution.FancyHash.Calculate(token);
}
