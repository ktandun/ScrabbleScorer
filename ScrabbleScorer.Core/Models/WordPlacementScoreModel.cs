using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public record WordPlacementScoreModel
{
    public required string Word { get; set; }
    public required Coordinate Coordinate { get; set; }
    public required Alignment Alignment { get; set; }
    public required int Score { get; set; }
}
