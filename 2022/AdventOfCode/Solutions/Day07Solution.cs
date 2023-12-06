using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions;

internal sealed partial class Day07Solution : SolutionBase
{
    private static readonly Regex CommandExpression = CommandRegex();

    private static readonly Regex FileExpression = FileRegex();
    
    private readonly Stack<Directory> _currentPath = new();

    private bool _isListing;

    private Directory CurrentDirectory =>
        _currentPath.Peek();
    
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        Solve(LoadDirectoryTree(input));

    private static bool IsCommand(string line) =>
        line.StartsWith("$ ");

    private static bool IsSubDirectory(string line) =>
        line.StartsWith("dir ");

    private static SolutionResult Solve(Directory directory) =>
        BuildResult(SumSmallDirectories(directory), SmallestLargeEnough(directory, 70000000, 30000000));

    private static long SmallestLargeEnough(Directory directory, long totalSize, long requiredAvailableSpace)
    {
        long freeSpace = totalSize - directory.Size;
        long missingSpace = requiredAvailableSpace - freeSpace;

        return Flatten(directory).Select(d => d.Size).OrderBy(s => s).First(s => s >= missingSpace);
    }

    private static SolutionResult BuildResult(long smallSizeSum, long smallestLargeEnough) =>
        ($"Sum of total directory sizes at most 100'000: {smallSizeSum}.",
            $"Smallest directory size that is large enough: {smallestLargeEnough}.");

    private static long SumSmallDirectories(Directory directory) =>
        Flatten(directory).Select(d => d.Size).Where(d => d <= 100000).Sum();

    private static IEnumerable<Directory> Flatten(Directory root)
    {
        var pending = new Stack<Directory>();
        pending.Push(root);

        while (pending.TryPop(out Directory? current))
        {
            foreach (Directory child in current) pending.Push(child);
            yield return current;
        }
    }

    private Directory LoadDirectoryTree(IReadOnlyList<string> input)
    {
        _currentPath.Clear();
        _isListing = false;

        foreach (string line in input) ProcessLineOrThrow(line);

        return _currentPath.Last();
    }

    private void ProcessLineOrThrow(string line)
    {
        if (!ProcessLine(line))
        {
            throw new InvalidOperationException();
        }
    }

    private bool ProcessLine(string line) =>
        IsCommand(line) ? ProcessCommand(line) : ProcessOutput(line);

    private bool ProcessCommand(string line) =>
        TerminateListing() && ProcessCommand(CommandExpression.Match(line));

    private bool ProcessCommand(Match match) =>
        match.Success && ProcessCommand(match.Groups);

    private bool ProcessCommand(GroupCollection command) =>
        ProcessCommand(command[1].Value, command[2].Value);

    private bool ProcessCommand(string command, string argument) =>
        command switch
        {
            "cd" => ChangeDirectoryTo(argument),
            "ls" => ListDirectory(),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };

    private bool ChangeDirectoryTo(string path) =>
        path switch
        {
            "/" => ChangeToRootDirectory(),
            ".." => ChangeToParentDirectory(),
            _ => ChangeToSubdirectory(path)
        };

    private bool ChangeToSubdirectory(string path)
    {
        _currentPath.Push(CurrentDirectory.Open(path));
        return true;
    }

    private bool ChangeToParentDirectory()
    {
        _currentPath.Pop();
        return _currentPath.Count > 0;
    }

    private bool ChangeToRootDirectory()
    {
        if (!_currentPath.Any()) _currentPath.Push(new Directory("/"));
        
        while (_currentPath.Count > 1) _currentPath.Pop();
        
        return true;
    }

    private bool ListDirectory() =>
        _isListing = _currentPath.Any();

    private bool ProcessOutput(string line) =>
        _isListing && ProcessListing(line);

    private bool ProcessListing(string line) =>
        IsSubDirectory(line) ? AddSubdirectory(line) : AddFile(line);

    private bool AddFile(string line) =>
        AddFile(FileExpression.Match(line));

    private bool AddFile(Match match) =>
        match.Success && AddFile(match.Groups);

    private bool AddFile(GroupCollection groups) =>
        AddFile(groups[2].Value, int.Parse(groups[1].Value));

    private bool AddFile(string file, int size) =>
        CurrentDirectory.AddFile(new File(file, size));

    private bool AddSubdirectory(string line)
    {
        _ = CurrentDirectory.Open(line.Replace("dir ", string.Empty));
        return true;
    }

    private bool TerminateListing() =>
        !(_isListing = false);

    private sealed class Directory : IEnumerable<Directory>
    {
        private readonly List<File> _files = new();
        
        private readonly string _name;
        
        private readonly List<Directory> _subDirectories = new();

        internal Directory(string name)
        {
            _name = name;
        }

        public long Size =>
            _subDirectories.Select(d => d.Size).Sum() + _files.Select(f => f.Size).Sum();

        public IEnumerator<Directory> GetEnumerator() =>
            _subDirectories.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        internal bool AddFile(File file)
        {
            if (_files.Any(f => f.Name == file.Name)) return false;

            _files.Add(file);
            return true;
        }

        internal Directory Open(string path) =>
            _subDirectories.SingleOrDefault(d => d._name == path) ?? Add(new Directory(path));

        private Directory Add(Directory directory)
        {
            _subDirectories.Add(directory);
            return directory;
        }
    }

    private readonly record struct File(string Name, long Size);

    [GeneratedRegex(@"^\$ (\w+)(?: ([^\s]+))?$")]
    private static partial Regex CommandRegex();

    [GeneratedRegex(@"^(\d+) ([^\s]+)$")]
    private static partial Regex FileRegex();
}
