namespace ScrabbleScorer.Core.Logic.Rules;

public interface IPlacementRule
{
    int Order { get; }
    bool Validate(Board board, PlacementModel placement);
}
