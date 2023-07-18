using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public record WordPlacementModel
{
    public required string Word { get; init; }
    public required Alignment Alignment { get; init; }
    public required Coordinate Coordinate { get; init; }
}
