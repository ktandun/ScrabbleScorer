using ScrabbleScorer.Core.Constants;

namespace ScrabbleScorer.Core.Logic.Rules;

public class WordShouldFitInsideBoard : IPlacementRule
{
    public bool Validate(Board board, PlacementModel placement)
    {
        var currentCoordinate = placement.Coordinate;

        for (var i = 0; i < placement.Letters.Count; i++)
        {
            var letter = board.GetLetterInCoordinate(currentCoordinate);

            if (letter is not null)
            {
                i--;
            }

            currentCoordinate = currentCoordinate.NextTile(placement.Alignment);
        }

        return currentCoordinate
            is {
                X: <= BoardCoordinateConstants.BoardSize,
                Y: <= BoardCoordinateConstants.BoardSize
            };
    }
}
