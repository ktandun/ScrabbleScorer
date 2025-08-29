namespace ScrabbleScorer.Core.Utilities;

public static class WordUtility
{
    public static string ToWord(this List<Letter> letters)
    {
        var word = new string(letters.Select(l => l.ToChar()).ToArray());

        return word;
    }
}