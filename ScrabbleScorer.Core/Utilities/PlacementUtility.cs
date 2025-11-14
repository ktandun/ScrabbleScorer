using ScrabbleScorer.Core.Logic.Rules;

namespace ScrabbleScorer.Core.Utilities;

public static class PlacementUtility
{
    extension(PlacementModel placement)
    {
        public bool IsTouchingCoordinate(Board board,
            Coordinate coordinate
        )
        {
            var currCoord = placement.Coordinate;

            for (var i = 0; i < placement.Letters.Length; i++)
            {
                while (board.GetLetterInCoordinate(currCoord) is not null)
                    currCoord = currCoord.NextTile(placement.Alignment);

                if (currCoord == coordinate)
                    return true;

                currCoord = currCoord.NextTile(placement.Alignment);
            }

            return false;
        }

        public bool IsTouchingOtherLetters(Board board)
        {
            var oppositeAlignment = placement.Alignment.Opposite();

            var allNeighbourCoordinates = new List<Coordinate>();

            var beforeFirstTile = placement.Coordinate.PrevTile(placement.Alignment);
            var afterLastTile = placement.Coordinate.NextTile(placement.Alignment, placement.Letters.Length);

            allNeighbourCoordinates.Add(beforeFirstTile);

            for (var i = 0; i < placement.Letters.Length; i++)
            {
                allNeighbourCoordinates.Add(placement.Coordinate.NextTile(placement.Alignment, i).PrevTile(oppositeAlignment));
                allNeighbourCoordinates.Add(placement.Coordinate.NextTile(placement.Alignment, i));
                allNeighbourCoordinates.Add(placement.Coordinate.NextTile(placement.Alignment, i).NextTile(oppositeAlignment));
            }

            allNeighbourCoordinates.Add(afterLastTile);

            var isTouching = allNeighbourCoordinates.Any(c => board.GetLetterInCoordinate(c) is not null);

            return isTouching;
        }
    }
}