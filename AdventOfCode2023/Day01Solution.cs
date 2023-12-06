using System.Runtime.CompilerServices;
[assembly:InternalsVisibleTo("AdventOfCode2023.Tests")]

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

    internal static int DumpSecondPuzzle(IReadOnlyList<string> input)
    {
        List<string> digits = input.Select(ConvertDigits).ToList();
        for (var i = 0; i < digits.Count; i++)
        {
            Console.WriteLine($"{i:D4} {digits[i]} {input[i]}");
        }

        Console.WriteLine(string.Join(", ", digits));
        return SolveFirstPuzzle(digits);
    }

    internal static int SolveSecondPuzzle(IEnumerable<string> input) =>
        SolveFirstPuzzle(input.Select(ConvertDigits));

    internal static string ConvertDigits(string line)
    {
        List<string> replaced = Numbers.Select((n, i) => line.Replace(n, $"{i + 1}")).ToList();

        int maxLength = replaced.Select(r => r.Length).Max();
        char first = ScanFirst(maxLength, replaced);
        char last = ScanLast(maxLength, replaced);

        return $"{first}{last}";
    }

    private static char ScanLast(int maxLength, IReadOnlyList<string> replaced)
    {
        for (int i = 0; i < maxLength; i++)
        {
            for (int j = 0; j < replaced.Count; j++)
            {
                int k = replaced[j].Length - i - 1;
                if (k >= 0 && char.IsDigit(replaced[j][k]))
                {
                    return replaced[j][k];
                }
            }
        }

        return '0';
    }

    private static char ScanFirst(int maxLength, IReadOnlyList<string> replaced)
    {
        for (int i = 0; i < maxLength; i++)
        {
            for (int j = 0; j < replaced.Count; j++)
            {
                if (i < replaced[j].Length && char.IsDigit(replaced[j][i]))
                {
                    return replaced[j][i];
                }
            }
        }

        return '0';
    }

    private static int BuildNumber(string line)
    {
        List<int> digits = line.Where(char.IsDigit).Select(c => c - '0').ToList();
        return digits.First() * 10 + digits.Last();
    }
}
