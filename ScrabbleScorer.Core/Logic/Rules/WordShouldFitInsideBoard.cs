namespace ScrabbleScorer.Core.Logic.Rules;

public class WordShouldFitInsideBoard : IPlacementRule
{
    public int Order => 2;

    public bool Validate(Board board, PlacementModel placement)
    {
        var (firstCoordinate, finalCoordinate, _) = board.TryPlaceLetters(placement);

        return finalCoordinate is { X: <= BoardConstants.BoardSize, Y: <= BoardConstants.BoardSize }
            && firstCoordinate is { X: >= 1, Y: >= 1 };
    }
}
