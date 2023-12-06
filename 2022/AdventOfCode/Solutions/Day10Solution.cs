namespace AdventOfCode.Solutions;

internal sealed class Day10Solution : SolutionBase
{
    private readonly char[] _buffer = new char[40 * 6];

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

        return BuildResult(_cumulatedStrengths, Dump(_buffer));
    }

    private static SolutionResult BuildResult(int sumOfStrengths, string dump) =>
        ($"Sum of signal strengths: {sumOfStrengths}", "Screen Snapshot:\n" + dump);

    private static string Dump(IEnumerable<char> buffer) =>
        string.Join('\n', buffer.Chunk(40).Select(c => new string(c.ToArray())));

    private static bool IsNoop(string line) =>
        line == "noop";

    private static bool IsSignificant(int cycle) =>
        (cycle + 20) % 40 == 0;

    private static bool IsVisible(int cycle, int position) =>
        cycle >= position - 1 && cycle <= position + 1;

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
        _buffer[_cycle % (40 * 6)] = IsVisible(_cycle % 40, _x) ? '#' : '.';

        _cycle++;
        if (IsSignificant(_cycle)) Cumulate(_cycle * _x);
    }
}
