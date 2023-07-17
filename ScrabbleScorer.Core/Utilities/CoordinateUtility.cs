using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Extensions;
using ScrabbleScorer.Core.Models;

namespace ScrabbleScorer.Core.Utilities;

public static class CoordinateUtility
{
    public static bool CanMakeWordOnCoordinate(
        Coordinate[] occupiedCoordinates,
        Coordinate coordinate,
        Alignment alignment,
        int wordLength
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
        var hasEmptyCoordinate = false;

        while (true)
        {
            delta++;

            if (!hasEmptyCoordinate && !occupiedCoordinates.Contains(currCoordinate))
                hasEmptyCoordinate = true;

            isAdjacentToOtherWord =
                isAdjacentToOtherWord
                || IsAdjacentCoordinateHasLetter(occupiedCoordinates, currCoordinate);

            if (delta >= wordLength)
                break;

            if (currCoordinate.Next(alignment, peek: true).IsWithinBoardDimensions())
                currCoordinate = currCoordinate.Next(alignment);
        }

        return isWithinBoardDimensions && isAdjacentToOtherWord && hasEmptyCoordinate;
    }

    private static bool IsAdjacentCoordinateHasLetter(
        Coordinate[] occupiedCoordinates,
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
