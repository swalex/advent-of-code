namespace AdventOfCode2023.Tests;

internal static class ExtensionsForExampleData
{
    internal static string Line(this string exampleData, int index) =>
        exampleData.Split("\n")[index];

    internal static IReadOnlyList<string> Lines(this string exampleData) =>
        exampleData.Split("\n");
}
