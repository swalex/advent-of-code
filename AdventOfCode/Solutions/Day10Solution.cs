namespace AdventOfCode.Solutions;

internal sealed class Day10Solution : SolutionBase
{
    private int _cumulatedStrengths;

    private int _cycle;

    private int _x;
    
    protected override SolutionResult Solve(IReadOnlyList<string> input)
    {
        _cumulatedStrengths = 0;
        _cycle = 0;
        _x = 1;

        foreach (string line in input)
        {
            if (IsNoop(line)) Tick();
            else ProcessAdd(line.Replace("addx ", string.Empty));
        }
        
        return BuildResult(_cumulatedStrengths, 0);
    }

    private static SolutionResult BuildResult(int sumOfStrengths, int bar) =>
        ($"Sum of signal strengths: {sumOfStrengths}", "Bar");

    private static bool IsNoop(string line) =>
        line == "noop";

    private static bool IsSignificant(int cycle) =>
        (cycle + 20) % 40 == 0;

    private void Cumulate(int value) =>
        _cumulatedStrengths += value;

    private void ProcessAdd(string value) =>
        ProcessAdd(int.Parse(value));

    private void ProcessAdd(int value)
    {
        Tick();
        Tick();
        _x += value;
    }

    private void Tick()
    {
        _cycle++;
        if (IsSignificant(_cycle)) Cumulate(_cycle * _x);
    }
}
