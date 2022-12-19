namespace AdventOfCode.Solutions;

internal sealed class Day06Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        Solve(input.Single());

    private static SolutionResult BuildResult(int position1, int position2) =>
        ($"First Marker after {position1} characters.",
            $"First Message after {position2} characters.");

    private static SolutionResult Solve(string sequence) =>
        BuildResult(Scan(sequence, 4), Scan(sequence, 14));

    private static int Scan(string sequence, int headerLength) =>
        new Scanner(headerLength).Scan(sequence);

    private sealed class Scanner
    {
        private readonly int _length;
        
        private readonly Queue<char> _values;

        private int _duplicates;

        internal Scanner(int headerLength)
        {
            _length = headerLength;
            _values = new Queue<char>(headerLength);
        }

        private bool FoundMarker =>
            _values.Count == _length && _duplicates == 0;

        internal int Scan(IEnumerable<char> data) =>
            data.TakeWhile(Push).Count() + 1;
        
        private bool Push(char value)
        {
            PopOldValue();
            PushNewValue(value);
            return !FoundMarker;
        }

        private void PopOldValue()
        {
            if (_values.Count < _length) return;

            char oldValue = _values.Dequeue();
            if (_values.Contains(oldValue)) _duplicates--;
        }

        private void PushNewValue(char value)
        {
            if (_values.Contains(value)) _duplicates++;
            _values.Enqueue(value);
        }
    }
}