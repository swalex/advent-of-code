namespace AdventOfCode;

internal readonly record struct SolutionResult(string FirstSolution, string SecondSolution)
{
    public static implicit operator SolutionResult((string First, string Second) solution) =>
        new(solution.First, solution.Second);
}