namespace AdventOfCode;

internal static class ExtensionsForILogger
{
    internal static void WriteLine(this ILogger logger, object value) =>
        logger.WriteLine(logger.Enabled ? value.ToString() ?? string.Empty : string.Empty);
}
