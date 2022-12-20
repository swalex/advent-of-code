namespace AdventOfCode;

internal readonly record struct Vector(int X, int Y)
{
    internal static Vector One { get; } = new(1, 1);

    internal Vector Direction =>
        Normalize(Length);

    internal int CoarseLength =>
        Math.Abs(X) + Math.Abs(Y);

    internal int Length =>
        (int)FloatLength;

    private float FloatLength =>
        MathF.Sqrt(X * X + Y * Y);

    public static explicit operator Vector(Position p) =>
        new(p.X, p.Y);

    public static Vector operator +(Vector a, Vector b) =>
        new(a.X + b.X, a.Y + b.Y);
    
    public static Vector operator /(Vector v, float length) =>
        new((int)MathF.Round(v.X / length, MidpointRounding.AwayFromZero),
            (int)MathF.Round(v.Y / length, MidpointRounding.AwayFromZero));

    internal static Vector Down(int distance) =>
        new(0, distance);

    internal static Vector Left(int distance) =>
        new(-distance, 0);

    internal static Vector Right(int distance) =>
        new(distance, 0);

    internal static Vector Up(int distance) =>
        new(0, -distance);

    private Vector Normalize(float length) =>
        length > 0 ? this / length : default;
}
