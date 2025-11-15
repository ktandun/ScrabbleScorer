namespace ScrabbleScorer.Core.Logic.Rules;

public class WordShouldFitInsideBoard : IPlacementRule
{
    public int Order => 2;

    public Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        var (firstCoordinate, finalCoordinate, _) = board.TryPlaceLetters(placement);

        return Task.FromResult(
            finalCoordinate is { X: <= BoardConstants.BoardSize, Y: <= BoardConstants.BoardSize }
                && firstCoordinate is { X: >= 1, Y: >= 1 }
        );
    }
}
