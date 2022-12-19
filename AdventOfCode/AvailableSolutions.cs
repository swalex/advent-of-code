namespace AdventOfCode;

internal static class AvailableSolutions
{
    private static readonly SolutionBase[] AllSolutions = GetAllSolutions(typeof(SolutionBase)).ToArray();

    internal static void SolveAll()
    {
        foreach (SolutionBase solution in AllSolutions)
        {
            Solve(solution);
        }
    }

    internal static void SolveLatest() =>
        Solve(AllSolutions.Last());

    private static void Solve(SolutionBase solution) =>
        WriteToConsole(solution, solution.Solve());

    private static void WriteToConsole(SolutionBase solution, SolutionResult result)
    {
        Console.WriteLine($"{solution.Key} Solution Results:");
        Console.WriteLine($"1/2 {result.FirstSolution}");
        Console.WriteLine($"2/2 {result.SecondSolution}");
        Console.WriteLine();
    }

    private static IEnumerable<SolutionBase> GetAllSolutions(Type baseType) =>
        baseType.Assembly
            .GetTypes()
            .Where(baseType.IsAssignableFrom)
            .Where(t => t.IsSealed)
            .Select(Activator.CreateInstance)
            .OfType<SolutionBase>()
            .OrderBy(s => s.Key);
}
