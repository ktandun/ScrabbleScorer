using System.Collections.Immutable;

namespace ScrabbleScorer.Core.Repositories;

public interface IWordRepository
{
    Task<DictionaryWords> GetDictionaryWordsOfLengthAsync(int length, char firstChar);
}

public class WordRepository : IWordRepository
{
    private readonly string _wordsFile = Path.Combine(
        AppContext.BaseDirectory,
        "Repositories",
        "words.txt"
    );
    private static Dictionary<int, Dictionary<char, DictionaryWords>>? _wordsMap;

    private async Task SetupAsync()
    {
        if (_wordsMap is not null)
        {
            return;
        }

        var allScrabbleWords = await File.ReadAllLinesAsync(_wordsFile);

        _wordsMap = allScrabbleWords
            .GroupBy(w => w.Length)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var words = g.ToArray();

                    return words
                        .GroupBy(w => w.First())
                        .ToDictionary(k => k.Key, v => new DictionaryWords(v.ToHashSet()));
                }
            );
    }

    public async Task<DictionaryWords> GetDictionaryWordsOfLengthAsync(int length, char firstChar)
    {
        await SetupAsync();

        return
            _wordsMap!.TryGetValue(length, out var words)
            && words.TryGetValue(firstChar, out var word)
            ? word
            : new DictionaryWords([]);
    }
}
