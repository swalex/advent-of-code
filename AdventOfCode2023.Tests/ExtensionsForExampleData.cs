namespace AdventOfCode2023.Tests;

internal static class ExtensionsForExampleData
{
    internal static IReadOnlyList<string> Lines(this string exampleData) =>
        exampleData.Split("\n");
}
