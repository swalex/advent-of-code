namespace AdventOfCode2023;

internal interface ISolution
{
    int Day { get; }

    long SolveFirstPuzzle(IReadOnlyList<string> input);

    long SolveSecondPuzzle(IReadOnlyList<string> input);
}
