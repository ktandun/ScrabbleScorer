namespace ScrabbleScorer.Core.Utilities;

public static class DictionaryWordsUtilities
{
    private static readonly WordEqualityComparer WordComparer = new();

    extension(DictionaryWords dictionaryWords)
    {
        public bool ShouldContain(string word)
        {
            return dictionaryWords.Words.Contains(word, WordComparer);
        }

        public bool ShouldContain(IEnumerable<string> words
        )
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            // Easier for debugging
            foreach (var word in words)
                if (!dictionaryWords.Words.Contains(word, WordComparer))
                    return false;

            return true;
        }
    }
}

public class WordEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (x is null || y is null || x.Length != y.Length)
            return false;

        for (var i = 0; i < x.Length; i++)
        {
            var xLetter = x[i].ToLetter();
            var yLetter = y[i].ToLetter();

            if (xLetter == yLetter)
                continue;

            if (xLetter == Letter.Blank || yLetter == Letter.Blank)
                continue;

            return false;
        }

        return true;
    }

    public int GetHashCode(string? obj)
    {
        return obj is null ? 0 : obj.Trim().ToLowerInvariant().GetHashCode();
    }
}