namespace ScrabbleScorer.Core.Logic.Rules;

public class TileShouldBeEmpty : IPlacementRule
{
    public bool Validate(Board board, PlacementModel placement)
    {
        var letter = board.GetLetterInCoordinate(placement.Coordinate);

        return letter is null;
    }
}