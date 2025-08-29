namespace ScrabbleScorer.Core.Logic.Rules;

public class TileShouldBeEmpty : IPlacementRule
{
    public Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        var letter = board.GetLetterInCoordinate(placement.Coordinate);

        return Task.FromResult(letter is null);
    }
}