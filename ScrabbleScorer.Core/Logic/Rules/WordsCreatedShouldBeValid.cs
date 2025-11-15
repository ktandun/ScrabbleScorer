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

        var attemptedPlacement = board.TryPlaceLetters(placement);
        var word = attemptedPlacement.wordCreated.Select(l => l.Letter).ToList().ToWord();

        var mainWordInDictionary = dictionaryWords.FindFirstMatching(word);

        if (mainWordInDictionary is null)
        {
            return false;
        }

        var newPlacement = new PlacementModel
        {
            Coordinate = attemptedPlacement.firstCoordinate,
            Alignment = placement.Alignment,
            Letters = mainWordInDictionary.Select(mw => mw.ToLetter()).ToArray()
        };

        var oppositeAlignmentWords = board
            .GetCreatedWordsOppositeAlignment(newPlacement)
            .Where(w => w.ToWord().Length > 1)
            .Select(w => w.ToWord())
            .ToList();

        return dictionaryWords.ShouldContain(oppositeAlignmentWords);
    }
}
