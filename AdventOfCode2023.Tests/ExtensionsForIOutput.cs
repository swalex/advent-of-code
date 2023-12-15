using System.Text;
using Xunit.Abstractions;

namespace AdventOfCode2023.Tests;

internal static class ExtensionsForIOutput
{
    internal static IOutput AsOutput(this ITestOutputHelper helper) =>
        new TestOutput(helper);

    private sealed class TestOutput : IOutput
    {
        private readonly StringBuilder _currentLine = new();

        private readonly ITestOutputHelper _helper;

        internal TestOutput(ITestOutputHelper helper)
        {
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public void Write(char c) =>
            _currentLine.Append(c);

        public void WriteLine() =>
            _helper.WriteLine(TakeLine());

        private string TakeLine()
        {
            var result = _currentLine.ToString();
            _currentLine.Clear();
            return result;
        }
    }
}
