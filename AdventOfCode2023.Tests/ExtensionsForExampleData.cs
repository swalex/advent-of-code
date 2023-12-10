namespace AdventOfCode2023.Tests;

internal static class ExtensionsForExampleData
{
    internal static string Line(this string exampleData, int index) =>
        Lines(exampleData)[index];

    internal static IReadOnlyList<string> Lines(this string exampleData) =>
        exampleData.Split("\r\n");
}
