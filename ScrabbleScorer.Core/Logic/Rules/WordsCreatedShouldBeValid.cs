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

    public bool Validate(Board board, PlacementModel placement)
    {
        var placements = GenerateAllPossiblePlacementsReplacingBlanks(placement);

        return placements.Select(p => ValidateSinglePlacement(board, p)).All(result => result);
    }

    private static IEnumerable<PlacementModel> GenerateAllPossiblePlacementsReplacingBlanks(
        PlacementModel placement
    )
    {
        if (placement.Letters.Contains(Letter.Blank))
        {
            var numberOfBlanks = placement.Letters.Count(l => l == Letter.Blank);
            var placementLettersWithoutBlanks = placement
                .Letters.Where(l => l != Letter.Blank)
                .ToArray();

            switch (numberOfBlanks)
            {
                case 1:
                    var blankOne = (
                        BoardConstants
                            .AllLettersWithoutBlank.Select(i =>
                                (Letter[])[.. placementLettersWithoutBlanks, i]
                            )
                            .Select(newLetters => placement with { Letters = newLetters })
                    );
                    foreach (var l in blankOne)
                        yield return l;
                    break;
                case 2:
                    var blankTwo = (
                        from i in BoardConstants.AllLettersWithoutBlank
                        from j in BoardConstants.AllLettersWithoutBlank
                        select (Letter[])[.. placementLettersWithoutBlanks, i, j] into newLetters
                        select placement with
                        {
                            Letters = newLetters
                        }
                    );
                    foreach (var l in blankTwo)
                        yield return l;
                    break;
            }
        }
        else
        {
            yield return placement;
        }
    }

    private bool ValidateSinglePlacement(Board board, PlacementModel placement)
    {
        var attemptedPlacement = board.TryPlaceLetters(placement);
        var word = attemptedPlacement.wordCreated.Select(l => l.Letter).ToList().ToWord();
        var dictionaryWord = _wordRepository.CheckWordInDictionary(word);

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

        return oppositeAlignmentWords
            .Select(_wordRepository.CheckWordInDictionary)
            .All(isValid => isValid is not null);
    }
}
