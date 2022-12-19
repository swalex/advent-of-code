var calories = new List<int> { 0 };

foreach (string line in File.ReadAllLines("input.txt"))
{
    if (string.IsNullOrEmpty(line))
    {
        calories.Add(0);
    }
    else
    {
        calories[^1] = calories[^1] + int.Parse(line);
    }
}

Console.WriteLine($"1/2 Most Calories Carried: {calories.MaxBy(i => i)}");
Console.WriteLine($"2/2 Total Carried Calories by the Top 3: {calories.OrderByDescending(i=>i).Take(3).Sum()}");
