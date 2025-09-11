namespace ScrabbleScorer.Core.Models;

public record Board
{
    public required BoardLetter[] BoardLetters { get; init; }

    public Letter? GetLetterInCoordinate(Coordinate coordinate)
    {
        return BoardLetters.SingleOrDefault(x => x.Coordinate == coordinate)?.Letter;
    }
}

public record BoardLetter
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

public record DictionaryWords(IEnumerable<string> Words);
