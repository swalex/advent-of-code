namespace AdventOfCode;

internal readonly record struct Bounds(Position TopLeft, Position BottomRight)
{
    internal Bounds(Position position)
        : this(position, position)
    {
    }
    
    internal int Height =>
        BottomRight.Y - TopLeft.Y;

    internal int Width =>
        BottomRight.X - TopLeft.X;

    internal Size Size =>
        new(BottomRight - TopLeft + Vector.One);

    public static Bounds operator +(Bounds b, Position p) =>
        new(Min(b.TopLeft, p), Max(b.BottomRight, p));

    private static Position Max(Position a, Position b) =>
        new(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
    
    private static Position Min(Position a, Position b) =>
        new(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
}
