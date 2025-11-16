using System.Collections.Immutable;

namespace ScrabbleScorer.Core.Repositories;

public interface IWordRepository
{
    string? CheckWordInDictionary(string word);
}

public class WordRepository : IWordRepository
{
    private readonly string _wordsFile = Path.Combine(
        AppContext.BaseDirectory,
        "Repositories",
        "words.txt"
    );

    private static ImmutableHashSet<string>? _wordsMap;

    private void Setup()
    {
        if (_wordsMap is not null)
        {
            return;
        }

        var allScrabbleWords = File.ReadAllLines(_wordsFile);

        _wordsMap = allScrabbleWords.ToImmutableHashSet();
    }

    public string? CheckWordInDictionary(string word)
    {
        Setup();

        return _wordsMap!.Contains(word) ? word : null;
    }
}
