using ScrabbleScorer.Core.Repositories;
using ScrabbleScorer.Core.Utilities;

namespace ScrabbleScorer.Core.Logic.Rules;

public class WordCreatedShouldBeValid : IPlacementRule
{
    private readonly IWordRepository _wordRepository;

    public WordCreatedShouldBeValid(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public async Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        var validWords = await _wordRepository.ReadWordsAsync();

        var letters = GetExistingLettersBeforePlacement(board, placement);
        var currentCoordinate = placement.Coordinate;

        for (var i = 0; i < placement.Letters.Count; i++)
        {
            var letter = board.GetLetterInCoordinate(currentCoordinate);

            if (letter is not null)
            {
                letters.Add(letter.Value);
                i--;
            }
            else
            {
                letters.Add(placement.Letters[i]);
            }

            currentCoordinate = currentCoordinate.NextTile(placement.Alignment);
        }

        var word = letters.ToWord();

        return validWords.Contains(word);
    }

    private static List<Letter> GetExistingLettersBeforePlacement(Board board, PlacementModel placement)
    {
        var letters = new List<Letter>();
        var currentCoordinate = placement.Coordinate.PrevTile(placement.Alignment);

        while (true)
        {
            var letter = board.GetLetterInCoordinate(currentCoordinate);

            if (letter is not null)
            {
                letters.Add(letter.Value);
            }
            else
            {
                break;
            }

            currentCoordinate = currentCoordinate.PrevTile(placement.Alignment);
        }
        
        letters.Reverse();

        return letters;
    }
}