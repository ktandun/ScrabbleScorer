using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public record TopScoringWordModel
{
    public required int Score { get; init; }
    public required string Word { get; init; }
    public required Coordinate Coordinate { get; init; }
    public required Alignment Alignment { get; init; }
}
