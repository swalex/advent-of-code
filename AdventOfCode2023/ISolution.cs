namespace AdventOfCode2023;

internal interface ISolution
{
    int Day { get; }

    int SolveFirstPuzzle(IReadOnlyList<string> input);

    int SolveSecondPuzzle(IReadOnlyList<string> input);
}
