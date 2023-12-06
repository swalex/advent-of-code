namespace AdventOfCode2023;

internal static class Day01Solution
{
    private static readonly List<string> Numbers = new()
    {
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
    };

    internal static int SolveFirstPuzzle(IEnumerable<string> input) =>
        input.Select(BuildNumber).Sum();

    internal static int TestSecondPuzzle(IEnumerable<string> input)
    {
        List<string> digits = input.Select(ConvertDigits).ToList();
        Console.WriteLine(string.Join(", ", digits));
        return SolveFirstPuzzle(digits);
    }

    internal static int SolveSecondPuzzle(IEnumerable<string> input) =>
        SolveFirstPuzzle(input.Select(ConvertDigits));

    private static string ConvertDigits(string line) =>
        Numbers.Aggregate(line, ReplaceTextDigit);

    private static string ReplaceTextDigit(string current, string number) =>
        current.Replace(number, AsDigit(number));

    private static string AsDigit(string number) =>
        AsIntegerDigit(number).ToString();

    private static int AsIntegerDigit(string number) =>
        Numbers.IndexOf(number) + 1;

    private static int BuildNumber(string line)
    {
        List<int> digits = line.Where(char.IsDigit).Select(c => c - '0').ToList();
        return digits.First() * 10 + digits.Last();
    }
}
