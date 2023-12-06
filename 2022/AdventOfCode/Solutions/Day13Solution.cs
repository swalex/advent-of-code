namespace AdventOfCode.Solutions;

internal sealed class Day13Solution : SolutionBase
{
    private static readonly ILogger ComparisonLog = NoLogger.Instance;
    
    private static readonly ILogger DumpLog = NoLogger.Instance;
    
    private const int RightOrder = -1;

    private const int WrongOrder = 1;

    private const int Equal = 0;
    
    protected override SolutionResult Solve(IReadOnlyList<string> input)
    {
        var sum = 0;
        for (var i = 0; i < input.Count; i += 3)
        {
            sum += Compare(i / 3 + 1, Load(input[i]), Load(input[i + 1])) ? i / 3 + 1 : 0;
        }

        DumpPackets(input);

        IPacket[] dividerPackets =
        {
            Packet.LoadFrom("[[2]]"),
            Packet.LoadFrom("[[6]]")
        };
        
        List<IPacket> packets = input.Where(l => !string.IsNullOrEmpty(l)).Select(Load).ToList();
        packets.AddRange(dividerPackets);
        
        packets.Sort();

        int divider1 = packets.IndexOf(dividerPackets[0]) + 1;
        int divider2 = packets.IndexOf(dividerPackets[1]) + 1;

        return BuildResult(sum, divider1 * divider2);
    }

    private static void DumpPackets(IReadOnlyList<string> input)
    {
        for (var i = 0; i < input.Count; i += 3)
        {
            DumpLog.WriteLine(Load(input[i]));
            DumpLog.WriteLine(Load(input[i + 1]));
            DumpLog.WriteLine();
        }
    }

    private static SolutionResult BuildResult(int sum, int decoderKey) =>
        ($"Sum of pairs in right order: {sum}", $"Decoder key: {decoderKey}");

    private static bool Compare(int index, IPacket left, IPacket right)
    {
        ComparisonLog.WriteLine($"== Pair {index} ==");
        bool result = left.CompareTo(right, 0) <= 0;
        
        ComparisonLog.WriteLine();
        return result;
    }

    private static IPacket Load(string line) =>
        Packet.LoadFrom(line);

    private interface IPacket : IComparable<IPacket>
    {
        void AddPacket(IPacket packet);
        
        void AddValue(int value);
        
        int CompareTo(IPacket right, int indent);
    }

    private sealed class NoPacket : IPacket
    {
        private NoPacket()
        {
        }

        internal static IPacket Instance { get; } = new NoPacket();
        public void AddPacket(IPacket packet)
        {
            // n.a.
        }

        public void AddValue(int value) =>
            throw new InvalidOperationException();

        public int CompareTo(IPacket right, int indent) =>
            throw new InvalidOperationException();

        public int CompareTo(IPacket? other) =>
            other != null ? CompareTo(other, 0) : WrongOrder;

        public override string ToString() =>
            "X";
    }
    
    private sealed class Packet : IPacket
    {
        private readonly List<IPacket> _values = new();

        private Packet()
        {
        }
        
        private Packet(IPacket inner)
        {
            _values.Add(inner);
        }
        
        public void AddPacket(IPacket packet) =>
            _values.Add(packet);

        public void AddValue(int value) =>
            _values.Add(new Integer(value));

        public int CompareTo(IPacket? other) =>
            other != null ? CompareTo(other, 0) : WrongOrder;
        
        public int CompareTo(IPacket right, int indent)
        {
            if (right is not Packet packet)
            {
                ComparisonLog.Write(new string(' ', indent * 2));
                ComparisonLog.WriteLine($"- Compare {this} vs {right}");
                indent++;

                ComparisonLog.Write(new string(' ', indent * 2));
                right = packet = new Packet(right);
                ComparisonLog.WriteLine($"- Mixed types; convert right to {right} and retry comparison");
            }

            ComparisonLog.Write(new string(' ', indent * 2));
            ComparisonLog.WriteLine($"- Compare {this} vs {right}");

            for (var i = 0; i < Math.Max(_values.Count, packet._values.Count); i++)
            {
                if (i >= _values.Count)
                {
                    ComparisonLog.Write(new string(' ', (indent + 1) * 2));
                    ComparisonLog.WriteLine("- Left side ran out of items, so inputs are in the right order");
                    return RightOrder;
                }

                if (i >= packet._values.Count)
                {
                    ComparisonLog.Write(new string(' ', (indent + 1) * 2));
                    ComparisonLog.WriteLine("- Right side ran out of items, so inputs are not in the right order");
                    return WrongOrder;
                }
                
                int result = _values[i].CompareTo(packet._values[i], indent + 1);
                if (result != 0) return result;
            }

            return Equal;
        }

        public override string ToString() =>
            $"[{string.Join(',', _values)}]";

        internal static IPacket LoadFrom(string line)
        {
            var stack = new Stack<IPacket>();
            IPacket current = NoPacket.Instance;
            IPacket? last = default;
            var value = 0;
            var hasValue = false;
            var pending = false;
            
            foreach (char item in line)
            {
                switch (item)
                {
                case '[':
                    if (pending) throw new InvalidOperationException();
                    
                    stack.Push(current);
                    IPacket newPacket = new Packet();
                    current.AddPacket(newPacket);
                    current = newPacket;
                    break;
                
                case ']':
                    if (hasValue) current.AddValue(value);
                    last = current;
                    current = stack.Pop();
                    hasValue = false;
                    pending = true;
                    break;
                
                case >= '0' and <= '9':
                    int digit = item - '0';
                    value = (hasValue ? value * 10 : 0) + digit;
                    hasValue = true;
                    pending = true;
                    break;
                
                case ',':
                    if (!pending) throw new InvalidOperationException();
                    if (hasValue) current.AddValue(value);
                    hasValue = false;
                    pending = false;
                    break;
                }
            }

            if (stack.Any()) throw new InvalidOperationException();

            return last ?? throw new InvalidOperationException();
        }

        private sealed record Integer(int Value) : IPacket
        {
            public void AddPacket(IPacket packet) =>
                throw new InvalidOperationException();

            public void AddValue(int value) =>
                throw new InvalidOperationException();

            public int CompareTo(IPacket? other) =>
                other != null ? CompareTo(other, 0) : WrongOrder;

            public int CompareTo(IPacket right, int indent) =>
                right is Integer rightValue
                    ? Compare(Value, rightValue.Value, indent)
                    : ConvertAndCompare(right, indent);

            private int ConvertAndCompare(IPacket right, int indent)
            {
                ComparisonLog.Write(new string(' ', indent * 2));
                ComparisonLog.WriteLine($"- Compare {this} vs {right}");
                indent++;
                
                ComparisonLog.Write(new string(' ', indent * 2));
                IPacket left = new Packet(this);
                ComparisonLog.WriteLine($"- Mixed types; convert left to {left} and retry comparison");
                return left.CompareTo(right, indent);
            }

            private static int Compare(int left, int right, int indent)
            {
                ComparisonLog.Write(new string(' ', indent * 2));
                ComparisonLog.WriteLine($"- Compare {left} vs {right}");

                if (left == right) return Equal;

                ComparisonLog.Write(new string(' ', (indent + 1) * 2));

                if (left < right)
                {
                    ComparisonLog.WriteLine("- Left side is smaller, so inputs are in the right order");
                    return RightOrder;
                }
               
                ComparisonLog.WriteLine("- Right side is smaller, so inputs are not in the right order");
                return WrongOrder;

            }

            public override string ToString() =>
                Value.ToString();
        }
    }
}
