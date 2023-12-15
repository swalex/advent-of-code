namespace AdventOfCode2023;

internal sealed class ConsoleOutput : IOutput
{
    private ConsoleOutput()
    {
    }

    internal static readonly IOutput Instance = new ConsoleOutput();

    public void Write(char c) =>
        Console.Write(c);

    public void WriteLine() =>
        Console.WriteLine();
}
