using Microsoft.Extensions.Caching.Memory;
using ScrabbleScorer.Core.Constants;

namespace ScrabbleScorer.Core.Repositories;

public interface IWordRepository
{
    Task<DictionaryWords> ReadWordsAsync();
}

public class WordRepository : IWordRepository
{
    private const string WordsFile = "./Repositories/words.txt";
    private readonly IMemoryCache _cache;

    public WordRepository(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<DictionaryWords> ReadWordsAsync()
    {
        if (_cache.TryGetValue(CacheKeys.Words, out var words)
            && words?.GetType() == typeof(List<string>))
            return new DictionaryWords((List<string>)words);

        var allScrabbleWords = await File.ReadAllLinesAsync(WordsFile);

        _cache.Set(CacheKeys.Words, allScrabbleWords);

        return new DictionaryWords(allScrabbleWords);
    }
}