namespace AdventOfCode2023;

public sealed class LatestSolution : ISolution
{
    private readonly ISolution _latest;

    public LatestSolution()
    {
        _latest = GetType().Assembly.DefinedTypes
            .Where(t => t.ImplementedInterfaces.Contains(typeof(ISolution)))
            .Where(t => t != GetType())
            .Select(Activator.CreateInstance)
            .OfType<ISolution>()
            .OrderBy(s => s.Day)
            .Where(NotSuspended)
            .Last();
    }

    private static bool NotSuspended(ISolution solution) =>
        solution.Day < 15;

    public int Day =>
        _latest.Day;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        _latest.SolveFirstPuzzle(input);

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        _latest.SolveSecondPuzzle(input);
}
