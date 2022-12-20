namespace AdventOfCode.Solutions;

internal sealed class Day11Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        Solve(BuildMonkeys(input).ToList());

    private static SolutionResult BuildResult(int businessLevel, int bar) =>
        ($"Level of Monkey Business after 20 Rounds: {businessLevel}", "Bar");

    private static IEnumerable<Monkey> BuildMonkeys(IEnumerable<string> input)
    {
        var builder = new MonkeyBuilder();
        foreach (string line in input)
        {
            if (builder.FeedInput(line)) yield return builder.BuildMonkey();
        }

        yield return builder.BuildMonkey();
    }

    private static SolutionResult Solve(List<Monkey> monkeys)
    {
        for (var round = 0; round < 20; round++)
        {
            foreach (Monkey monkey in monkeys)
            {
                monkey.InspectItemsAndThrowTo(monkeys);
            }
        }

        List<int> capos = monkeys.Select(m => m.PerformedInspections).OrderByDescending(i => i).Take(2).ToList();

        return BuildResult(capos[0] * capos[1], 0);
    }

    private sealed class Monkey
    {
        private readonly Queue<int> _items = new();

        private readonly Func<int, int> _operation;

        private readonly Func<int, bool> _test;

        private readonly int _positiveTarget;

        private readonly int _negativeTarget;

        internal Monkey(Func<int, int> operation, Func<int, bool> test, int positiveTarget, int negativeTarget,
            IEnumerable<int> items)
        {
            foreach (int item in items)
            {
                _items.Enqueue(item);
            }

            _operation = operation ?? throw new ArgumentNullException(nameof(operation));
            _test = test ?? throw new ArgumentNullException(nameof(test));
            _positiveTarget = positiveTarget;
            _negativeTarget = negativeTarget;
        }

        internal int PerformedInspections { get; private set; }

        internal void InspectItemsAndThrowTo(IReadOnlyList<Monkey> monkeys)
        {
            while (_items.TryDequeue(out int itemWorryLevel))
            {
                itemWorryLevel = _operation.Invoke(itemWorryLevel);
                itemWorryLevel /= 3;
                monkeys[GetTarget(itemWorryLevel)]._items.Enqueue(itemWorryLevel);
                PerformedInspections++;
            }
        }

        private int GetTarget(int itemWorryLevel) =>
            _test.Invoke(itemWorryLevel) ? _positiveTarget : _negativeTarget;
    }

    private sealed class MonkeyBuilder
    {
        private readonly List<int> _items = new();

        private int _currentId;

        private int _currentStep;
        
        private Func<int,int>? _operation;
        private Func<int,bool>? _test;
        private int _positiveTarget;
        private int _negativeTarget;

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
            _operation = DecodeOperation(line.Replace("  Operation: new = old ", string.Empty).Split(' '));

        private void LoadTest(string line) =>
            _test = DecodeTest(int.Parse(line.Replace("  Test: divisible by ", string.Empty)));

        private void LoadPositiveTarget(string line) =>
            _positiveTarget = int.Parse(line.Replace("    If true: throw to monkey ", string.Empty));

        private void LoadNegativeTarget(string line) =>
            _negativeTarget = int.Parse(line.Replace("    If false: throw to monkey ", string.Empty));

        private static Func<int, bool> DecodeTest(int value) =>
            stress => stress % value == 0;

        private static Func<int, int> DecodeOperation(IReadOnlyList<string> parts) =>
            old => DecodeOperator(parts[0])(old, DecodeOperand(parts[1])(old));

        private static Func<int, int> DecodeOperand(string value) =>
            value switch
            {
                "old" => old => old,
                _ => _ => int.Parse(value)
            };

        private static Func<int, int, int> DecodeOperator(string value) =>
            value switch
            {
                "+" => (old, operand) => old + operand,
                "*" => (old, operand) => old * operand,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };

        private void LoadItems(string line) =>
            _items.AddRange(line.Replace("  Starting items: ", string.Empty)
                .Split(',', StringSplitOptions.TrimEntries)
                .Select(int.Parse));

        internal Monkey BuildMonkey() =>
            new(_operation!, _test!, _positiveTarget, _negativeTarget, _items);

        private void VerifyId(string line)
        {
            if (line != $"Monkey {_currentId}:") throw new InvalidOperationException();
            _currentId++;
            _items.Clear();
        }
    }
}
