using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Extensions;
using ScrabbleScorer.Core.Models;

namespace ScrabbleScorer.Core.Utilities;

public static class CoordinateUtility
{
    public static bool CanMakeWordOnCoordinate(
        HashSet<Coordinate> occupiedCoordinates,
        Coordinate coordinate,
        Alignment alignment,
        int wordLength,
        int lettersOnHandCount
    )
    {
        var lastLetterCoordinate = new Coordinate(
            alignment == Alignment.Horizontal ? coordinate.X + (wordLength - 1) : coordinate.X,
            alignment == Alignment.Vertical ? coordinate.Y - (wordLength - 1) : coordinate.Y
        );

        if (!lastLetterCoordinate.IsWithinBoardDimensions())
            return false;

        var currCoordinate = coordinate;
        var delta = 0;

        var isWithinBoardDimensions = true;
        var isAdjacentToOtherWord = false;
        var hasUnoccupiedCoordinates = false;
        var isBoardEmpty = occupiedCoordinates.Count == 0;
        var isPassingMiddlePoint = false;
        var middlePoint = new Coordinate(8, 8);

        while (true)
        {
            delta++;

            if (currCoordinate == middlePoint)
                isPassingMiddlePoint = true;

            if (!occupiedCoordinates.Contains(currCoordinate))
            {
                hasUnoccupiedCoordinates = true;
                lettersOnHandCount--;
            }

            isAdjacentToOtherWord =
                isAdjacentToOtherWord
                || IsAdjacentCoordinateHasLetter(occupiedCoordinates, currCoordinate);

            if (delta >= wordLength || lettersOnHandCount == 0)
                break;

            if (currCoordinate.Next(alignment, peek: true).IsWithinBoardDimensions())
                currCoordinate = currCoordinate.Next(alignment);
        }

        return isBoardEmpty
            ? isPassingMiddlePoint && isWithinBoardDimensions
            : isWithinBoardDimensions && isAdjacentToOtherWord && hasUnoccupiedCoordinates;
    }

    private static bool IsAdjacentCoordinateHasLetter(
        HashSet<Coordinate> occupiedCoordinates,
        Coordinate coordinate
    ) =>
        occupiedCoordinates.Any(
            c =>
                c == new Coordinate(coordinate.X + 1, coordinate.Y)
                || c == new Coordinate(coordinate.X - 1, coordinate.Y)
                || c == new Coordinate(coordinate.X, coordinate.Y + 1)
                || c == new Coordinate(coordinate.X, coordinate.Y - 1)
        );
}
