namespace AdventOfCode.Tests;

public class TestPosition
{
    [Fact]
    public void DiagonalDirection()
    {
        var vector = new Vector(1, 2);

        Assert.Equal(new Vector(1, 1), vector.Direction);
    }

    [Fact]
    public void DiagonalLength()
    {
        var vector = new Vector(1, 2);

        Assert.Equal(2, vector.Length);
    }
}
