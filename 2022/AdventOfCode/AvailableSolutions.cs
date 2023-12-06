namespace AdventOfCode;

internal static class AvailableSolutions
{
    private static readonly SolutionBase[] AllSolutions = GetAllSolutions(typeof(SolutionBase)).ToArray();

    internal static void SolveAll()
    {
        foreach (SolutionBase solution in AllSolutions)
        {
            solution.Solve();
        }
    }

    internal static void SolveLatest() =>
        AllSolutions.Last().Solve();

    private static IEnumerable<SolutionBase> GetAllSolutions(Type baseType) =>
        baseType.Assembly
            .GetTypes()
            .Where(baseType.IsAssignableFrom)
            .Where(t => t.IsSealed)
            .Select(Activator.CreateInstance)
            .OfType<SolutionBase>()
            .OrderBy(s => s.Key);
}
