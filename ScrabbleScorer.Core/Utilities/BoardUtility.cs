using ScrabbleScorer.Core.Logic.Rules;

namespace ScrabbleScorer.Core.Utilities;

public static class BoardUtility
{
    public static Coordinate[] GetAllCoordinatesWithinDistance(Coordinate coordinate, int distance)
    {
        var coordinates = new HashSet<Coordinate>();


        foreach (var alignment in BoardCoordinateConstants.AllAlignments)
        {
            for (var i = 1; i <= distance; i++)
            {
                coordinates.Add(coordinate.NextTile(alignment, i));
                coordinates.Add(coordinate.NextTile(alignment).NextTile(alignment.Opposite(), i-1));
                coordinates.Add(coordinate.PrevTile(alignment.Opposite(), i));
                coordinates.Add(coordinate.PrevTile(alignment.Opposite()).PrevTile(alignment, i-1));
            }
        }

        return coordinates.ToArray();
    }

    extension(Board board)
    {
        public bool IsEmpty()
        {
            return board.BoardLetters.Length == 0;
        }

        public Letter? GetLetterInCoordinate(Coordinate coordinate)
        {
            return board.BoardLetters.SingleOrDefault(x => x.Coordinate == coordinate)?.Letter;
        }


        public (Coordinate finalCoordinate, List<LetterOnBoard> wordCreated) TryPlaceLetters(PlacementModel placement
        )
        {
            var currCoord = placement.Coordinate;
            var letters = new List<LetterOnBoard>();

            foreach (var letter in placement.Letters)
            {
                while (board.GetLetterInCoordinate(currCoord) is not null)
                {
                    letters.Add(new LetterOnBoard
                    {
                        Letter = board.GetLetterInCoordinate(currCoord)!.Value,
                        Coordinate = currCoord
                    });
                    currCoord = currCoord.NextTile(placement.Alignment);
                }

                letters.Add(new LetterOnBoard
                {
                    Letter = letter,
                    Coordinate = currCoord
                });

                currCoord = currCoord.NextTile(placement.Alignment);
            }

            var wordCreated = board
                .GetPlacementPrefixLetters(placement)
                .Concat(letters)
                .Concat(board.GetPlacementSuffixLetters(currCoord, placement.Alignment))
                .ToList();

            return (currCoord.PrevTile(placement.Alignment), wordCreated);
        }

        public List<List<LetterOnBoard>> GetCreatedWordsOppositeAlignment(PlacementModel placement
        )
        {
            var oppositeAlignment = placement.Alignment.Opposite();

            var letters = placement
                .Letters.Select((letter, index) =>
                    GetCreatedWordOfAlignment(
                        board,
                        placement with
                        {
                            Coordinate = placement.Coordinate.NextTile(placement.Alignment, index),
                            Alignment = oppositeAlignment,
                            Letters = [letter]
                        }
                    )
                )
                .ToList();

            return letters.ToList();
        }

        public List<LetterOnBoard> GetCreatedWordOfAlignment(PlacementModel placement)
        {
            var (_, letters) = board.TryPlaceLetters(placement);

            return letters;
        }

        private List<LetterOnBoard> GetPlacementPrefixLetters(PlacementModel placement
        )
        {
            var letters = new List<LetterOnBoard>();
            var currentCoordinate = placement.Coordinate.PrevTile(placement.Alignment);

            while (true)
            {
                var letter = board.GetLetterInCoordinate(currentCoordinate);

                if (letter is not null)
                    letters.Add(new LetterOnBoard
                    {
                        Letter = letter.Value,
                        Coordinate = currentCoordinate
                    });
                else
                    break;

                currentCoordinate = currentCoordinate.PrevTile(placement.Alignment);
            }

            letters.Reverse();

            return letters;
        }

        private List<LetterOnBoard> GetPlacementSuffixLetters(Coordinate lastLetterCoordinate,
            Alignment alignment
        )
        {
            var letters = new List<LetterOnBoard>();
            var currentCoordinate = lastLetterCoordinate;

            while (true)
            {
                var letter = board.GetLetterInCoordinate(currentCoordinate);

                if (letter is not null)
                    letters.Add(new LetterOnBoard
                    {
                        Letter = letter.Value,
                        Coordinate = currentCoordinate
                    });
                else
                    break;

                currentCoordinate = currentCoordinate.NextTile(alignment);
            }

            return letters;
        }
    }
}