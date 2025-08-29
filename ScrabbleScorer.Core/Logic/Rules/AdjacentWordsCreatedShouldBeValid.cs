using ScrabbleScorer.Core.Repositories;

namespace ScrabbleScorer.Core.Logic.Rules;

public class AdjacentWordsCreatedShouldBeValid : IPlacementRule
{
    private readonly IWordRepository _wordRepository;

    public AdjacentWordsCreatedShouldBeValid(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }
    
    public async Task<bool> ValidateAsync(Board board, PlacementModel placement)
    {
        throw new NotImplementedException();
    }
}