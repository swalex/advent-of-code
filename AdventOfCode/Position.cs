namespace AdventOfCode;

internal readonly record struct Position(int X, int Y)
{
    public static Position operator -(Position p) =>
        new(-p.X, -p.Y);

    public static Vector operator -(Position a, Position b) =>
        new(a.X - b.X, a.Y - b.Y);
    
    public static Position operator +(Position p, Vector v) =>
        new(p.X + v.X, p.Y + v.Y);

    internal static Position Zero =>
        default;
}
