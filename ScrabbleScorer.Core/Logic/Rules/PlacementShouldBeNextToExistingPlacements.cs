namespace ScrabbleScorer.Core.Logic.Rules;

public class PlacementShouldBeNextToExistingPlacements : IPlacementRule
{
    public int Order => 3;

    public bool Validate(Board board, PlacementModel placement)
    {
        var isValid = board.IsEmpty() switch
        {
            true => placement.IsTouchingCoordinate(board, BoardConstants.CentreTile),
            false => placement.IsTouchingOtherLetters(board),
        };

        return isValid;
    }
}
