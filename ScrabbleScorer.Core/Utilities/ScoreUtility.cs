using System.Diagnostics;
using ScrabbleScorer.Core.Constants;
using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Models;

namespace ScrabbleScorer.Core.Utilities;

public static class ScoreUtility
{
    public static int CalculateScore(string letters, BonusType[]? bonuses, bool isBingo)
    {
        if (bonuses is not null)
            Debug.Assert(letters.Length == bonuses.Length);

        var sum = 0;
        var multiplier = 1;

        for (var i = 0; i < letters.Length; i++)
        {
            var letter = letters[i];
            var bonus = bonuses is not null ? bonuses[i] : BonusType.None;

            switch (bonus)
            {
                case BonusType.None:
                    sum += letter.ToLetterValue();
                    break;
                case BonusType.DoubleWord:
                    multiplier *= 2;
                    sum += letter.ToLetterValue();
                    break;
                case BonusType.TripleWord:
                    multiplier *= 3;
                    sum += letter.ToLetterValue();
                    break;
                case BonusType.DoubleLetter:
                    sum += 2 * letter.ToLetterValue();
                    break;
                case BonusType.TripleLetter:
                    sum += 3 * letter.ToLetterValue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var bingoScore = isBingo ? 50 : 0;

        return multiplier * sum + bingoScore;
    }

    public static int CalculateScore(
        Board board,
        Coordinate coordinate,
        Alignment alignment,
        string letters
    )
    {
        var boardLetterCoordinates = board.BoardLetters.Select(bl => bl.Coordinate).ToArray();
        var (boardLetterAfterPlacement, isBingo) = BoardUtility.TryPlaceWord(
            board.BoardLetters,
            coordinate,
            alignment,
            letters
        );

        var wordPlacements = BoardUtility.FindCreatedWordsAfterPlacement(
            board.BoardLetters,
            boardLetterAfterPlacement
        );

        return (
            from wordPlacement in wordPlacements
            let bonusTypes = wordPlacement.Coordinates.Select(c =>
            {
                return boardLetterCoordinates.Contains(c)
                    ? BonusType.None
                    : BoardCoordinateConstants.BonusTiles
                        .FirstOrDefault(bt => bt.Coordinate == c)
                        ?.BonusType ?? BonusType.None;
            })
            select CalculateScore(wordPlacement.Word, bonusTypes.ToArray(), isBingo)
        ).Sum();
    }
}
