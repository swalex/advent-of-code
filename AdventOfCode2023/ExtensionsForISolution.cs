namespace AdventOfCode2023;

internal static class ExtensionsForISolution
{
    internal static void Solve(this ISolution solution) =>
        Solve(solution, File.ReadAllLines($"InputData/day{solution.Day:D2}.txt"));

    private static void Solve(ISolution solution, IReadOnlyList<string> input)
    {
        Console.WriteLine($"Day {solution.Day:D2} Puzzle");
        Console.WriteLine($"First puzzle:  {solution.SolveFirstPuzzle(input)}");
        Console.WriteLine($"Second puzzle: {solution.SolveSecondPuzzle(input)}");
    }
}
