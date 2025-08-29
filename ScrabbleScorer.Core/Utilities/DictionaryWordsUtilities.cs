namespace ScrabbleScorer.Core.Utilities;

public static class DictionaryWordsUtilities
{
    private static readonly WordEqualityComparer WordComparer = new();

    public static bool Contains(this DictionaryWords dictionaryWords, string word)
    {
        return dictionaryWords.Words.Contains(word, WordComparer);
    }
}

public class WordEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        if (x.Length != y.Length)
            return false;

        for (var i = 0; i < x.Length; i++)
        {
            var xLetter = x[i].ToLetter();
            var yLetter = y[i].ToLetter();

            if (xLetter == yLetter) continue;
            if (xLetter == Letter.Blank || yLetter == Letter.Blank) continue;

            return false;
        }

        return true;
    }

    public int GetHashCode(string? obj)
    {
        return obj is null ? 0 : obj.Trim().ToLowerInvariant().GetHashCode();
    }
}