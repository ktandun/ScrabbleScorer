namespace ScrabbleScorer.Core.Logic.Rules;

public class PlacementShouldBeNextToExistingPlacements : IPlacementRule
{
    public int Order => 2;

    public Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        var isValid = board.IsEmpty() switch
        {
            true => placement.IsTouchingCoordinate(board, BoardCoordinateConstants.CentreTile),
            false => placement.IsTouchingOtherLetters(board)
        };

        return Task.FromResult(isValid);
    }
}
