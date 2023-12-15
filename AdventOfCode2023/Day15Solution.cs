using System.Collections;

namespace AdventOfCode2023;

public sealed class Day15Solution : ISolution
{
    public int Day =>
        15;

    public long SolveFirstPuzzle(IReadOnlyList<string> input) =>
        input.Select(SolveFirstPuzzleOnLine).Sum();

    public long SolveSecondPuzzle(IReadOnlyList<string> input) =>
        input.Aggregate(0L, (acc, l) => acc + SolveSecondPuzzle(l));

    private static long SolveSecondPuzzle(string input)
    {
        Box[] boxes = Enumerable.Repeat(0, 256).Select(_ => new Box()).ToArray();

        foreach (string task in input.Split(','))
        {
            var lens = Lens.DecodeLens(task);
            if (lens.FocalLength < 0)
            {
                boxes[lens.BoxIndex].Remove(lens);
            }
            else
            {
                boxes[lens.BoxIndex].AddOrReplace(lens);
            }
        }

        return boxes.Select((b, bi) => (bi + 1) * b.Select((b, i) => b.FocalLength * (i + 1))
            .Aggregate(0, (i, l) => i + l)).Sum();
    }

    private static long SolveFirstPuzzleOnLine(string line) =>
        line.Split(',').Select(ComputeHash).Sum();

    private static long ComputeHash(string token) =>
        FancyHash.Calculate(token);

    internal static class FancyHash
    {
        internal static int Calculate(string token) =>
            token.Aggregate(0, Add);

        private static int Add(int value, char letter) =>
            (value + letter) * 17 % 256;
    }

    internal sealed class Box : IEnumerable<Lens>
    {
        private readonly LinkedList<Lens> _lenses = new();

        internal void AddOrReplace(Lens lens)
        {
            LinkedListNode<Lens>? node = _lenses.Find(lens);
            if (node != null) node.Value = lens;
            else _lenses.AddLast(lens);
        }

        internal void Remove(Lens lens) =>
            _lenses.Remove(lens);

        public IEnumerator<Lens> GetEnumerator() =>
            _lenses.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)_lenses).GetEnumerator();
    }

    internal sealed class Lens : IEquatable<Lens>, IComparable<Lens>
    {
        private Lens(string label, int focalLength)
        {
            Label = label;
            FocalLength = focalLength;
            BoxIndex = label.FancyHash();
        }

        internal int FocalLength { get; }

        internal int BoxIndex { get; }

        internal string Label { get; }

        internal static Lens DecodeLens(string code) =>
            code.Contains('=') ? DecodeLens(code.Split('=')) : new Lens(code.Split('-')[0], -1);

        private static Lens DecodeLens(IReadOnlyList<string> parts) =>
            new(parts[0], int.Parse(parts[1]));

        public bool Equals(Lens? other) =>
            Label.Equals(other?.Label);

        public int CompareTo(Lens? other) =>
            string.CompareOrdinal(Label, other?.Label);
    }
}

internal static class ExtensionsForFancyHash
{
    internal static int FancyHash(this string token) =>
        Day15Solution.FancyHash.Calculate(token);
}
