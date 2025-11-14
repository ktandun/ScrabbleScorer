using ScrabbleScorer.Core.Repositories;

namespace ScrabbleScorer.Core.Logic.Rules;

public class WordsCreatedShouldBeValid : IPlacementRule
{
    private readonly IWordRepository _wordRepository;

    public WordsCreatedShouldBeValid(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public int Order => 4;

    public async Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        var dictionaryWords = await _wordRepository.GetDictionaryWordsAsync();

        var word = board.GetCreatedWordOfAlignment(placement).ToWord();
        var oppositeAlignmentWords = board
            .GetCreatedWordsOppositeAlignment(placement)
            .Where(w => w.ToWord().Length > 1)
            .Select(w => w.ToWord())
            .ToList();

        return placement.Letters.Length switch
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
