namespace AdventOfCode;

internal readonly record struct Size(int Width, int Height)
{
    internal Size(Vector v)
        : this(v.X, v.Y)
    {
    }
}
