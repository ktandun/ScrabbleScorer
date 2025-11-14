namespace ScrabbleScorer.Core.Logic.Rules;

public class WordShouldFitInsideBoard : IPlacementRule
{
    public int Order => 3;

    public Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        var (finalCoordinate, _) = board.TryPlaceLetters(placement);

        return Task.FromResult(
            finalCoordinate
                is {
                    X: <= BoardCoordinateConstants.BoardSize,
                    Y: <= BoardCoordinateConstants.BoardSize
                }
        );
    }
}
