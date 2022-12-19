namespace AdventOfCode.Solutions;

internal sealed class Day02Solution : SolutionBase
{
    protected override SolutionResult Solve(IReadOnlyList<string> input) =>
        BuildResult(input.Select(DetermineScore1).Sum(), input.Select(DetermineScore2).Sum());

    private static SolutionResult BuildResult(int score1, int score2) =>
        ($"Total Score with Rock/Paper/Scissors Mapping: {score1}",
            $"Total Score with Lose/Draw/Win Mapping: {score2}");

    private static int DetermineScore1(string line) =>
        DetermineScore1(line[0], line[2]);

    private static int DetermineScore1(char opponentChoice, char myChoice) =>
        DetermineScore(TranslateOpponentChoice(opponentChoice), TranslateMyChoice(myChoice));

    private static int DetermineScore2(string line) =>
        DetermineScore2(line[0], line[2]);

    private static int DetermineScore2(char opponentChoice, char myResult) =>
        DetermineScore2(TranslateOpponentChoice(opponentChoice), TranslateMyResult(myResult));

    private static int DetermineScore2(Choice opponentChoice, Result myResult) =>
        GetResultScore(myResult) + GetChoiceScore(DetermineChoice(opponentChoice, myResult));

    private static Choice DetermineChoice(Choice opponentChoice, Result myResult) =>
        myResult == Result.Draw ? opponentChoice : GetNonDrawChoice(opponentChoice, myResult == Result.Won);

    private static Choice GetNonDrawChoice(Choice opponentChoice, bool shallWin) =>
        opponentChoice switch
        {
            Choice.Rock => shallWin ? Choice.Paper : Choice.Scissors,
            Choice.Paper => shallWin ? Choice.Scissors : Choice.Rock,
            Choice.Scissors => shallWin ? Choice.Rock : Choice.Paper,
            _ => throw new ArgumentOutOfRangeException(nameof(opponentChoice), opponentChoice, null)
        };

    private static Choice TranslateOpponentChoice(char value) =>
        value switch
        {
            'A' => Choice.Rock,
            'B' => Choice.Paper,
            'C' => Choice.Scissors,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    private static Choice TranslateMyChoice(char value) =>
        value switch
        {
            'X' => Choice.Rock,
            'Y' => Choice.Paper,
            'Z' => Choice.Scissors,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    private static Result TranslateMyResult(char value) =>
        value switch
        {
            'X' => Result.Lost,
            'Y' => Result.Draw,
            'Z' => Result.Won,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    private static int DetermineScore(Choice opponentChoice, Choice myChoice) =>
        GetChoiceScore(myChoice) + GetResultScore(opponentChoice, myChoice);

    private static int GetChoiceScore(Choice value) =>
        value switch
        {
            Choice.Rock => 1,
            Choice.Paper => 2,
            Choice.Scissors => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    private static int GetResultScore(Choice opponentChoice, Choice myChoice) =>
        GetResultScore(GetResult(opponentChoice, myChoice));

    private static Result GetResult(Choice opponentChoice, Choice myChoice) =>
        opponentChoice == myChoice ? Result.Draw : HasWon(opponentChoice, myChoice) ? Result.Won : Result.Lost;

    private static bool HasWon(Choice opponentChoice, Choice myChoice) =>
        opponentChoice switch
        {
            Choice.Rock => myChoice == Choice.Paper,
            Choice.Paper => myChoice == Choice.Scissors,
            Choice.Scissors => myChoice == Choice.Rock,
            _ => throw new ArgumentOutOfRangeException(nameof(opponentChoice), opponentChoice, null)
        };

    private static int GetResultScore(Result value) =>
        value switch
        {
            Result.Lost => 0,
            Result.Draw => 3,
            Result.Won => 6,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    private enum Choice
    {
        Rock,
        Paper,
        Scissors
    }

    private enum Result
    {
        Lost,
        Draw,
        Won
    }
}
