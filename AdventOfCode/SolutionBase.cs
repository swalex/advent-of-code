namespace AdventOfCode;

internal abstract class SolutionBase
{
    internal string Key =>
        GetType().Name.Replace("Solution", string.Empty);

    private string InputFileName =>
        Path.Combine("input", $"{Key.ToLowerInvariant()}.txt");

    internal SolutionResult Solve() =>
        Solve(File.ReadAllLines(InputFileName));

    protected abstract SolutionResult Solve(IReadOnlyList<string> input);
}
