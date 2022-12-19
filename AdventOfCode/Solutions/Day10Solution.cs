namespace AdventOfCode.Solutions;

internal sealed class Day10Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        BuildResult(0, 0);

    private static SolutionResult BuildResult(int foo, int bar) =>
        ("Foo", "Bar");
}
