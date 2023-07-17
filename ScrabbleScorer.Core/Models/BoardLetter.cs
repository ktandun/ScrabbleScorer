using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public record BoardLetter
{
    public required Letter Letter { get; init; }
    public required Coordinate Coordinate { get; init; }
}
