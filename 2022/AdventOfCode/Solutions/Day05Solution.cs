using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions;

internal sealed partial class Day05Solution : SolutionBase
{
    private static readonly Regex MoveExpression = MoveRegex();

    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        Solve(LoadSetup(input));

    private SolutionResult Solve((List<Stack<char>> Stacks, List<Move> Moves) setup)
    {
        List<Stack<char>> stacks2 = setup.Stacks.Select(s => new Stack<char>(s.Reverse())).ToList();
        
        foreach (Move move in setup.Moves)
        {
            ApplyMove1(move, setup.Stacks);
        }

        foreach (Move move in setup.Moves)
        {
            ApplyMove2(move, stacks2);
        }

        return BuildResult(ToString(setup.Stacks), ToString(stacks2));
    }

    private static string ToString(IEnumerable<Stack<char>> sequence) =>
        ToString(sequence.Select(s => s.Peek()));

    private static string ToString(IEnumerable<char> sequence) =>
        new(sequence.ToArray());
    
    private static SolutionResult BuildResult(string topCrates1, string topCrates2) =>
        ($"Top Crates when moved one by one: {topCrates1}",
            $"Top Crates when moved all at once: {topCrates2}");

    private static void ApplyMove1(Move move, IReadOnlyList<Stack<char>> stacks)
    {
        for (var i = 0; i < move.Amount; i++)
        {
            stacks[move.To - 1].Push(stacks[move.From - 1].Pop());
        }
    }

    private static void ApplyMove2(Move move, IReadOnlyList<Stack<char>> stacks)
    {
        var buffer = new Stack<char>();
        
        for (var i = 0; i < move.Amount; i++)
        {
            buffer.Push(stacks[move.From - 1].Pop());
        }

        while (buffer.Any()) stacks[move.To - 1].Push(buffer.Pop());
    }

    private static (List<Stack<char>>, List<Move>) LoadSetup(IReadOnlyCollection<string> lines) =>
        LoadSetup(lines.IndexOf(string.Empty), lines);

    private static (List<Stack<char>>, List<Move>) LoadSetup(int separator, IReadOnlyCollection<string> lines) =>
        (LoadStacks(lines.Take(separator).Reverse().ToList()), LoadMoves(lines.Skip(separator + 1)).ToList());

    private static IEnumerable<Move> LoadMoves(IEnumerable<string> lines) =>
        lines.Select(l => MoveExpression.Match(l))
            .Where(m => m.Success)
            .Select(m=>m.Groups)
            .Select(MoveFromMatch);

    private static Move MoveFromMatch(GroupCollection groups) =>
        MoveFromMatch(groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToList());

    private static Move MoveFromMatch(IReadOnlyList<int> values) =>
        new(values[0], values[1], values[2]);

    private static List<Stack<char>> LoadStacks(IReadOnlyList<string> lines)
    {
        List<int> positions = lines[0]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
        List<Stack<char>> stacks = Enumerable.Range(0, positions.Max()).Select(_ => new Stack<char>()).ToList();
        List<int> offsets = positions.Select(p => p.ToString())
            .Select(s => lines[0].IndexOf(s, StringComparison.Ordinal)).ToList();

        foreach (string line in lines.Skip(1))
        {
            for (var i = 0; i < offsets.Count; i++)
            {
                if (line.Length <= offsets[i]) break;

                char crate = line[offsets[i]];
                if (crate is >= 'A' and <= 'Z') stacks[positions[i] - 1].Push(crate);
            }
        }

        return stacks;
    }

    private readonly record struct Move(int Amount, int From, int To);

    [GeneratedRegex("^move (\\d+) from (\\d+) to (\\d+)$")]
    private static partial Regex MoveRegex();
}
