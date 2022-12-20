namespace AdventOfCode;

internal sealed class ConsoleLogger : ILogger
{
    private ConsoleLogger()
    {
    }
    
    internal static ILogger Instance { get; } = new ConsoleLogger();

    public bool Enabled =>
        true;

    public void Write(string text) =>
        Console.Write(text);

    public void WriteLine() =>
        Console.WriteLine();

    public void WriteLine(string text) =>
        Console.WriteLine(text);
}
