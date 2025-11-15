namespace ScrabbleScorer.Core.Models;

public record Board
{
    public LetterOnBoard[] BoardLetters { get; init; } = [];
}

public record LetterOnBoard
{
    public required Letter Letter { get; init; }
    public required Coordinate Coordinate { get; init; }
}

public record Coordinate(int X, int Y);

public record BonusTile
{
    public required Coordinate Coordinate { get; init; }
    public required BonusType BonusType { get; init; }
}

public record DictionaryWords(HashSet<string> Words)
{
    public string? FindFirstMatching(string word)
    {
        return Words.Contains(word) ? word : null;
    }
}

public record PlacementScoreModel(PlacementModel Placement, int Score);

public record PlacementModel
{
    public required Coordinate Coordinate { get; init; }
    public required Alignment Alignment { get; init; }
    public required IReadOnlyList<Letter> Letters { get; init; } = [];
}
