using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public record BonusTile
{
    public required Coordinate Coordinate { get; init; }
    public required BonusType BonusType { get; init; }
}