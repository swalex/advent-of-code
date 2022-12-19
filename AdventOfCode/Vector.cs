namespace AdventOfCode;

internal readonly record struct Vector(int X, int Y)
{
    internal Vector Direction =>
        Normalize(Length);

    internal int Length =>
        (int)FloatLength;

    private float FloatLength =>
        MathF.Sqrt(X * X + Y * Y);

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
