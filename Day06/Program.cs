namespace Day06;

internal static class Program
{
    private static void Main()
    {
        string sequence = File.ReadAllText("input.txt");

        int position1 = new Scanner(4).Scan(sequence);
        int position2 = new Scanner(14).Scan(sequence);

        Console.WriteLine($"1/2 First Marker after {position1} characters.");
        Console.WriteLine($"2/2 First Message after {position2} characters.");
    }

    private sealed class Scanner
    {
        private int _length;
        
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