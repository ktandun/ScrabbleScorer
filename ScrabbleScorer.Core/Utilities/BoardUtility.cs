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
        var currentCoordinate = placement.Coordinate.NextTile(placement.Alignment, count: placement.Letters.Count);

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
}