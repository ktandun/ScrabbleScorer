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
    public Coordinate NextTile(Alignment alignment, int count = 1)
    {
        return alignment == Alignment.Horizontal
            ? this with { X = X + count }
            : this with { Y = Y + count };
    }
    
    public Coordinate PrevTile(Alignment alignment, int count = 1)
    {
        return alignment == Alignment.Horizontal
            ? this with { X = X - count }
            : this with { Y = Y - count };
    }
}