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

public record Coordinate(int X, int Y)
{
    public Coordinate NextTile(Alignment alignment)
    {
        return alignment == Alignment.Horizontal
            ? this with { X = X + 1 }
            : this with { Y = Y + 1 };
    }
    
    public Coordinate PrevTile(Alignment alignment)
    {
        return alignment == Alignment.Horizontal
            ? this with { X = X - 1 }
            : this with { Y = Y - 1 };
    }
}