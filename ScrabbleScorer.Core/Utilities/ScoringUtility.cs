using ScrabbleScorer.Core.Logic.Rules;

namespace ScrabbleScorer.Core.Utilities;

public static class ScoringUtility
{
    public static PlacementScoreModel ScorePlacement(Board board, PlacementModel placement)
    {
        var allCreatedWords = new List<List<LetterOnBoard>>();

        var word = board.GetCreatedWordOfAlignment(placement);
        var oppositeAlignmentWords = board
            .GetCreatedWordsOppositeAlignment(placement)
            .Where(w => w.ToWord().Length > 1)
            .ToList();

        allCreatedWords.Add(word);
        allCreatedWords.AddRange(oppositeAlignmentWords);

        var totalScore = allCreatedWords.Sum(letters => CalculateWordScore(board, letters));

        if (placement.Letters.Length == 7)
        {
            totalScore += 50;
        }

        return new PlacementScoreModel(placement, totalScore);
    }

    private static int CalculateWordScore(Board board, List<LetterOnBoard> word)
    {
        var score = 0;

        var foundWordMultipliers = new List<BonusType>();

        foreach (var letter in word)
        {
            var existingLetter = board.GetLetterInCoordinate(letter.Coordinate);
            var bonusTile = BoardCoordinateConstants.BonusTiles.FirstOrDefault(bt =>
                bt.Coordinate == letter.Coordinate
            );

            if (existingLetter is null)
            {
                score += bonusTile?.BonusType switch
                {
                    BonusType.DL => letter.Letter.ToLetterValue() * 2,
                    BonusType.TL => letter.Letter.ToLetterValue() * 3,
                    _ => letter.Letter.ToLetterValue()
                };
            }
            else
            {
                score += existingLetter.Value.ToLetterValue();
            }

            if (existingLetter is null && bonusTile is { BonusType: BonusType.DW or BonusType.TW })
            {
                foundWordMultipliers.Add(bonusTile.BonusType);
            }
        }

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var multipliers in foundWordMultipliers)
        {
            score *= multipliers switch
            {
                BonusType.DW => 2,
                BonusType.TW => 3,
                _ => 1
            };
        }

        return score;
    }
}
