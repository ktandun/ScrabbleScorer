namespace ScrabbleScorer.Core.Utilities;

public static class WordUtility
{
    public static string ToWord(this List<Letter> letters)
    {
        var word = new string(letters.Select(l => l.ToChar()).ToArray());

        return word;
    }

    public static string ToWord(this IReadOnlyList<Letter> letters)
    {
        var word = new string(letters.Select(l => l.ToChar()).ToArray());

        return word;
    }

    public static string ToWord(this List<LetterOnBoard> letters)
    {
        var word = new string(letters.Select(l => l.Letter).ToList().ToWord());

        return word;
    }
}
