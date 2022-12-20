namespace AdventOfCode;

internal sealed class NoLogger : ILogger
{
    private NoLogger()
    {
    }
    
    internal static ILogger Instance { get; } = new NoLogger();

    public bool Enabled =>
        false;

    public void Write(string text)
    {
        // n.a.
    }

    public void WriteLine()
    {
        // n.a.
    }

    public void WriteLine(string text)
    {
        // n.a.
    }
}
