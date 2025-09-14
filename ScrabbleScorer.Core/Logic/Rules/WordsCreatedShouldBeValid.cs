using ScrabbleScorer.Core.Repositories;
using ScrabbleScorer.Core.Utilities;

namespace ScrabbleScorer.Core.Logic.Rules;

public class WordsCreatedShouldBeValid : IPlacementRule
{
    private readonly IWordRepository _wordRepository;

    public WordsCreatedShouldBeValid(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public async Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        var dictionaryWords = await _wordRepository.GetDictionaryWordsAsync();

        var word = BoardUtility.GetCreatedWordOfAlignment(board, placement);
        var oppositeAlignmentWords = BoardUtility
            .GetCreatedWordsOppositeAlignment(board, placement)
            .Where(w => w.Length > 1)
            .ToList();

        return placement.Letters.Count switch
        {
            1
                => dictionaryWords.ShouldContain(word)
                    || dictionaryWords.ShouldContain(oppositeAlignmentWords),
            _
                => dictionaryWords.ShouldContain(word)
                    && dictionaryWords.ShouldContain(oppositeAlignmentWords)
        };
    }
}
