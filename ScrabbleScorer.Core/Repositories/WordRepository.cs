namespace ScrabbleScorer.Core.Repositories;

public interface IWordRepository
{
    Task<DictionaryWords> GetDictionaryWordsAsync();
}

public class WordRepository : IWordRepository
{
    private readonly string _wordsFile = Path.Combine(
        AppContext.BaseDirectory,
        "Repositories",
        "words.txt"
    );
    private static DictionaryWords? _words;

    public async Task<DictionaryWords> GetDictionaryWordsAsync()
    {
        if (_words?.Words.Length > 0)
        {
            return _words;
        }

        var allScrabbleWords = await File.ReadAllLinesAsync(_wordsFile);
        _words = new DictionaryWords(allScrabbleWords);
        return _words;
    }
}
