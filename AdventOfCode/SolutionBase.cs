using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AdventOfCode.Tests")]

namespace AdventOfCode;

internal abstract class SolutionBase
{
    internal string Key =>
        GetType().Name.Replace("Solution", string.Empty);

    private string InputFile =>
        Path.Combine("input", $"{Key.ToLowerInvariant()}.txt");

    private IEnumerable<string> TestFiles =>
        Directory.Exists("test")
            ? Directory.EnumerateFiles("test", $"{Key.ToLowerInvariant()}*.txt").OrderBy(f => f)
            : Array.Empty<string>();

    internal void Solve()
    {
        foreach (string testFile in TestFiles) Solve(testFile);
        Solve(InputFile);
    }

    protected abstract SolutionResult Solve(IReadOnlyList<string> input);

    private void Solve(string file) =>
        WriteToConsole(file, Solve(File.ReadAllLines(file)));
    
    private void WriteToConsole(string file, SolutionResult result)
    {
        Console.WriteLine($"{Key} Solution Results for {file}:");
        Console.WriteLine($"1/2 {result.FirstSolution}");
        Console.WriteLine($"2/2 {result.SecondSolution}");
        Console.WriteLine();
    }
}
