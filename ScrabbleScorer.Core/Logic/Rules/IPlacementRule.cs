namespace ScrabbleScorer.Core.Logic.Rules;

public interface IPlacementRule
{
    bool Validate(Board board, PlacementModel placement);
}

public record PlacementModel
{
    public required Coordinate Coordinate { get; init; }
    public required Alignment Alignment { get; init; }
    public required List<Letter> Letters { get; init; } = [];
}