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
        var validWords = await _wordRepository.ReadWordsAsync();

        var word = BoardUtility.GetCreatedWordOfAlignment(board, placement);
        var words = BoardUtility
            .GetCreatedWordsOppositeAlignment(board, placement)
            .Where(w => w.Length > 1)
            .ToList();

        return placement.Letters.Count switch
        {
            1 => validWords.Contains(word) || validWords.Contains(words),
            _ => validWords.Contains(word) && validWords.Contains(words)
        };
    }
}
