namespace ScrabbleScorer.Core.Logic.Rules;

public interface IPlacementRule
{
    int Order { get; }
    Task<bool> ValidateAsync(Board board, PlacementModel placement);
}

