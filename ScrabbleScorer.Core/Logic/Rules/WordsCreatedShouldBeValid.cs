using System.Collections.Immutable;
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
        var placements = new List<PlacementModel>();
        if (placement.Letters.Contains(Letter.Blank))
        {
            var numberOfBlanks = placement.Letters.Count(l => l == Letter.Blank);
            var placementLettersWithoutBlanks = placement
                .Letters.Where(l => l != Letter.Blank)
                .ToArray();

            switch (numberOfBlanks)
            {
                case 1:
                    placements.AddRange(
                        BoardConstants
                            .AllLettersWithoutBlank.Select(i =>
                                (Letter[])[.. placementLettersWithoutBlanks, i]
                            )
                            .Select(newLetters => placement with { Letters = newLetters })
                    );
                    break;
                case 2:
                    placements.AddRange(
                        from i in BoardConstants.AllLettersWithoutBlank
                        from j in BoardConstants.AllLettersWithoutBlank
                        select (Letter[])[.. placementLettersWithoutBlanks, i, j] into newLetters
                        select placement with
                        {
                            Letters = newLetters
                        }
                    );
                    break;
            }
        }
        else
        {
            placements.Add(placement);
        }

        foreach (var p in placements)
        {
            var result = await ValidateSinglePlacementAsync(board, p);

            if (!result)
            {
                return false;
            }
        }

        return true;
    }

    private async Task<bool> ValidateSinglePlacementAsync(Board board, PlacementModel placement)
    {
        var attemptedPlacement = board.TryPlaceLetters(placement);
        var word = attemptedPlacement.wordCreated.Select(l => l.Letter).ToList().ToWord();

        var dictionaryWord = (
            await _wordRepository.GetDictionaryWordsOfLengthAsync(word.Length, word.First())
        ).FindFirstMatching(word);

        if (dictionaryWord is null)
        {
            return false;
        }

        var newPlacement = new PlacementModel
        {
            Coordinate = attemptedPlacement.firstCoordinate,
            Alignment = placement.Alignment,
            Letters = dictionaryWord.Select(mw => mw.ToLetter()).ToArray(),
        };

        var oppositeAlignmentWords = board
            .GetCreatedWordsOppositeAlignment(newPlacement)
            .Where(w => w.ToWord().Length > 1)
            .Select(w => w.ToWord())
            .ToList();

        foreach (var oppositeAlignmentWord in oppositeAlignmentWords)
        {
            var isValid = (
                await _wordRepository.GetDictionaryWordsOfLengthAsync(
                    oppositeAlignmentWord.Length,
                    oppositeAlignmentWord.First()
                )
            ).FindFirstMatching(oppositeAlignmentWord);

            if (isValid is null)
            {
                return false;
            }
        }

        return true;
    }
}
