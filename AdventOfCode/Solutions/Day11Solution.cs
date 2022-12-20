namespace AdventOfCode.Solutions;

internal sealed class Day11Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        Solve(input, new MonkeyBuilder());

    private static SolutionResult Solve(IEnumerable<string> input, MonkeyBuilder builder) =>
        Solve(BuildMonkeys(input, builder).ToList(), builder);

    private static SolutionResult BuildResult(long easyBusinessLevel, long panicBusinessLevel) =>
        ($"Relaxed level of Monkey Business after 20 Rounds: {easyBusinessLevel}",
            $"Panic level of Monkey Business after 10000 Rounds: {panicBusinessLevel}");

    private static IEnumerable<Monkey> BuildMonkeys(IEnumerable<string> input, MonkeyBuilder builder)
    {
        foreach (string line in input)
        {
            if (builder.FeedInput(line)) yield return builder.BuildMonkey();
        }

        yield return builder.BuildMonkey();
    }

    private static SolutionResult Solve(IReadOnlyList<Monkey> monkeys, MonkeyBuilder builder) =>
        Solve(monkeys, builder.CommonDivisor);
    
    private static SolutionResult Solve(IReadOnlyList<Monkey> monkeys, long commonDivisor) =>
        BuildResult(CalculateMonkeyBusiness(monkeys.Select(m => m.Clone()).ToList(), stress => stress / 3, 20),
            CalculateMonkeyBusiness(monkeys, stress => stress % commonDivisor, 10000));

    private static long CalculateMonkeyBusiness(IReadOnlyList<Monkey> monkeys, Func<long, long> relax, int iterations)
    {
        for (var round = 1; round <= iterations; round++)
        {
            foreach (Monkey monkey in monkeys)
            {
                monkey.InspectItemsAndThrowTo(monkeys, relax);
            }

            if (ShouldDump(round)) Dump(round, monkeys);
        }

        List<long> capos = monkeys.Select(m => m.PerformedInspections).OrderByDescending(i => i)
            .Take(2)
            .ToList();

        return capos[0] * capos[1];
    }

    private static void Dump(int round, IEnumerable<Monkey> monkeys)
    {
        Console.WriteLine($"== After round {round} ==");
        foreach (Monkey monkey in monkeys) Console.WriteLine(monkey);

        Console.WriteLine();
    }

    private static bool ShouldDump(int round) =>
        round is 1 or 20 || round % 1000 == 0;

    private sealed class Monkey
    {
        private readonly Queue<long> _items = new();

        private readonly MonkeySetup _setup;

        internal Monkey(MonkeySetup setup, IEnumerable<long> items)
        {
            foreach (long item in items)
            {
                _items.Enqueue(item);
            }

            _setup = setup;
        }

        internal long PerformedInspections { get; private set; }

        public override string ToString() =>
            $"Monkey {_setup.Id} inspected items {PerformedInspections} times.";

        internal Monkey Clone() =>
            new(_setup, _items);
        
        internal void InspectItemsAndThrowTo(IReadOnlyList<Monkey> monkeys, Func<long, long> relax)
        {
            while (_items.TryDequeue(out long item))
            {
                item = _setup.Operation?.Invoke(item) ?? item;
                item = relax.Invoke(item);
                ThrowTo(monkeys, _setup.GetTarget(item), item);
                PerformedInspections++;
            }
        }

        private static void ThrowTo(IReadOnlyList<Monkey> monkeys, int target, long item) =>
            monkeys[target]._items.Enqueue(item);
    }

    private readonly record struct MonkeySetup(
        int Id,
        Func<long, long>? Operation,
        Func<long, bool>? Test,
        int PositiveTarget,
        int NegativeTarget)
    {
        internal int GetTarget(long itemWorryLevel) =>
            Test?.Invoke(itemWorryLevel) == true ? PositiveTarget : NegativeTarget;
    }

    private sealed class MonkeyBuilder
    {
        private readonly List<long> _items = new();

        private MonkeySetup _setup;
        
        private int _currentStep;

        internal long CommonDivisor { get; private set; } = 1;

        internal bool FeedInput(string line)
        {
            GetCurrentStep(_currentStep).Invoke(line);
            _currentStep = (_currentStep + 1) % 7;
            return _currentStep == 0;
        }

        private Action<string> GetCurrentStep(int step) =>
            step switch
            {
                0 => VerifyId,
                1 => LoadItems,
                2 => LoadOperation,
                3 => LoadTest,
                4 => LoadPositiveTarget,
                5 => LoadNegativeTarget,
                6 => SkipEmptyLine,
                _ => throw new ArgumentOutOfRangeException(nameof(step), step, null)
            };

        private static void SkipEmptyLine(string line)
        {
        }

        private void LoadOperation(string line) =>
            _setup = _setup with
            {
                Operation = DecodeOperation(line.Replace("  Operation: new = old ", string.Empty).Split(' '))
            };

        private void LoadTest(string line) =>
            _setup = _setup with
            {
                Test = DecodeTest(long.Parse(line.Replace("  Test: divisible by ", string.Empty)))
            };

        private void LoadPositiveTarget(string line) =>
            _setup = _setup with
            {
                PositiveTarget = int.Parse(line.Replace("    If true: throw to monkey ", string.Empty))
            };

        private void LoadNegativeTarget(string line) =>
            _setup = _setup with
            {
                NegativeTarget = int.Parse(line.Replace("    If false: throw to monkey ", string.Empty))
            };

        private Func<long, bool> DecodeTest(long value)
        {
            CommonDivisor *= value;
            return stress => stress % value == 0;
        }

        private static Func<long, long> DecodeOperation(IReadOnlyList<string> parts) =>
            old => DecodeOperator(parts[0])(old, DecodeOperand(parts[1])(old));

        private static Func<long, long> DecodeOperand(string value) =>
            value == "old" ? old => old : Constant(long.Parse(value));

        private static Func<long, long> Constant(long value) =>
            _ => value;

        private static Func<long, long, long> DecodeOperator(string value) =>
            value switch
            {
                "+" => (old, operand) => old + operand,
                "*" => (old, operand) => old * operand,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };

        private void LoadItems(string line) =>
            _items.AddRange(line.Replace("  Starting items: ", string.Empty)
                .Split(',', StringSplitOptions.TrimEntries)
                .Select(long.Parse));

        internal Monkey BuildMonkey() =>
            BuildMonkey(_setup);

        private Monkey BuildMonkey(MonkeySetup setup)
        {
            _setup = new MonkeySetup { Id = setup.Id + 1 };
            return new(setup, _items);
        }

        private void VerifyId(string line)
        {
            if (line != $"Monkey {_setup.Id}:") throw new InvalidOperationException();
            _items.Clear();
        }
    }
}
