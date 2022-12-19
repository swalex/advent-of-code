namespace AdventOfCode.Solutions;

internal sealed class Day01Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        BuildResult(CollectCalories(input));

    private static SolutionResult BuildResult(IReadOnlyCollection<int> calories) =>
        ($"Most Calories Carried: {calories.MaxBy(i => i)}",
            $"Total Carried Calories by the Top 3: {calories.OrderByDescending(i => i).Take(3).Sum()}");

    private static IReadOnlyCollection<int> CollectCalories(IEnumerable<string> input)
    {
        var calories = new List<int> { 0 };

        foreach (string line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                calories.Add(0);
            }
            else
            {
                calories[^1] += int.Parse(line);
            }
        }

        return calories;
    }
}
