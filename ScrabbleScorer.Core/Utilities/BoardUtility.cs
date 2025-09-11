using ScrabbleScorer.Core.Logic.Rules;

namespace ScrabbleScorer.Core.Utilities;

public static class BoardUtility
{
    public static List<Letter> GetPlacementPrefixLetters(this Board board, PlacementModel placement)
    {
        var letters = new List<Letter>();
        var currentCoordinate = placement.Coordinate.PrevTile(placement.Alignment);

        while (true)
        {
            var letter = board.GetLetterInCoordinate(currentCoordinate);

            if (letter is not null)
            {
                letters.Add(letter.Value);
            }
            else
            {
                break;
            }

            currentCoordinate = currentCoordinate.PrevTile(placement.Alignment);
        }

        letters.Reverse();

        return letters;
    }

    public static List<Letter> GetPlacementSuffixLetters(this Board board, PlacementModel placement)
    {
        var letters = new List<Letter>();
        var currentCoordinate = placement.Coordinate.NextTile(
            placement.Alignment,
            count: placement.Letters.Count
        );

        while (true)
        {
            var letter = board.GetLetterInCoordinate(currentCoordinate);

            if (letter is not null)
            {
                letters.Add(letter.Value);
            }
            else
            {
                break;
            }

            currentCoordinate = currentCoordinate.NextTile(placement.Alignment);
        }

        return letters;
    }

    public static IReadOnlyCollection<string> GetCreatedWordsOppositeAlignment(
        Board board,
        PlacementModel placement
    )
    {
        var oppositeAlignment = placement.Alignment.Opposite();

        var letters = placement
            .Letters.Select(
                (letter, index) =>
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

    public static string GetCreatedWordOfAlignment(Board board, PlacementModel placement)
    {
        var letters = new List<Letter>();
        var currentCoordinate = placement.Coordinate;

        for (var i = 0; i < placement.Letters.Count; i++)
        {
            var letter = board.GetLetterInCoordinate(currentCoordinate);

            if (letter is not null)
            {
                letters.Add(letter.Value);
                i--;
            }
            else
            {
                letters.Add(placement.Letters[i]);
            }

            currentCoordinate = currentCoordinate.NextTile(placement.Alignment);
        }

        var word = board
            .GetPlacementPrefixLetters(placement)
            .Concat(letters)
            .Concat(board.GetPlacementSuffixLetters(placement))
            .ToList()
            .ToWord();
        return word;
    }
}
