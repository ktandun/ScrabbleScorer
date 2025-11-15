using System.Collections.Concurrent;
using System.Diagnostics;
using ScrabbleScorer.Core.Logic.Rules;

namespace ScrabbleScorer.Core.Services;

public interface IGameService
{
    Task<List<PlacementScoreModel>> FindBestWordAsync(Board board, List<Letter> lettersOnHand);
}

public class GameService : IGameService
{
    private readonly IPlacementRule[] _calculationRules;

    public GameService(IEnumerable<IPlacementRule> rules)
    {
        var placementRules = rules as IPlacementRule[] ?? rules.ToArray();

        _calculationRules = placementRules.OrderBy(pr => pr.Order).ToArray();
    }

    public async Task<List<PlacementScoreModel>> FindBestWordAsync(
        Board board,
        List<Letter> lettersOnHand
    )
    {
        var allPlacements = GenerateAllPlacements(board, lettersOnHand);
        var results = await ValidatePlacementsAsync(board, allPlacements);
        var scoredPlacements = ScorePlacements(board, results);
        var highestNScores = scoredPlacements.OrderByDescending(sp => sp.Score).Take(10).ToList();
        return highestNScores;
    }

    private async Task<List<PlacementModel>> ValidatePlacementsAsync(
        Board board,
        IEnumerable<PlacementModel> allPlacements
    )
    {
        var validPlacements = new ConcurrentBag<PlacementModel>();

        await Parallel.ForEachAsync(
            allPlacements,
            new ParallelOptions { MaxDegreeOfParallelism = 8 },
            async (placement, token) =>
            {
                if (await ValidatePlacementAsync(board, placement) is not null)
                    validPlacements.Add(placement);
            }
        );

        var results = validPlacements.Distinct().ToList();
        return results;
    }

    private List<PlacementScoreModel> ScorePlacements(Board board, List<PlacementModel> placements)
    {
        return placements.Select(p => ScoringUtility.ScorePlacement(board, p)).ToList();
    }

    private IEnumerable<PlacementModel> GenerateAllPlacements(
        Board board,
        List<Letter> lettersOnHand
    )
    {
        return (
            from alignment in BoardCoordinateConstants.AllAlignments
            from letters in CombinationUtils
                .GetAllPermutationsOfAllSubsets(lettersOnHand)
                .Distinct()
            let lettersArray = letters.ToArray()
            from coordinate in board.BoardLetters.SelectMany(bl =>
                BoardUtility.GetAllCoordinatesWithinDistance(bl.Coordinate, lettersArray.Length)
            )
            select new PlacementModel
            {
                Coordinate = coordinate,
                Alignment = alignment,
                Letters = lettersArray
            }
        );
    }

    private async Task<PlacementModel?> ValidatePlacementAsync(
        Board board,
        PlacementModel placement
    )
    {
        foreach (var rule in _calculationRules)
        {
            if (!await rule.ValidateAsync(board, placement))
            {
                return null;
            }
        }

        return placement;
    }
}
