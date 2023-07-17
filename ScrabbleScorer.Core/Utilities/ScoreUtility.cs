using System.Diagnostics;
using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Utilities;

public static class ScoreUtility
{
    public static int CalculateScore(string letters, BonusType[]? bonuses = null)
    {
        if (bonuses is not null)
            Debug.Assert(letters.Length == bonuses.Length);

        var sum = 0;
        var multiplier = 1;

        for (var i = 0; i < letters.Length; i++)
        {
            var letter = letters[i];
            var bonus = bonuses is not null
                ? bonuses[i]
                : BonusType.None;

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

        return multiplier * sum;
    }
}