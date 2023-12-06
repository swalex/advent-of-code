namespace AdventOfCode;

internal interface ILogger
{
    bool Enabled { get; }
    
    void Write(string text);
    
    void WriteLine();
    
    void WriteLine(string text);
}
