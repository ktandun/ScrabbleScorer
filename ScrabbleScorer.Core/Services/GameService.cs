using System.Collections.Concurrent;
using System.Diagnostics;
using ScrabbleScorer.Core.Logic.Rules;

namespace ScrabbleScorer.Core.Services;

public interface IGameService
{
    IEnumerable<PlacementScoreModel> FindBestWord(Board board, List<Letter> lettersOnHand);
}

public class GameService : IGameService
{
    private readonly IPlacementRule[] _calculationRules;

    public GameService(IEnumerable<IPlacementRule> rules)
    {
        var placementRules = rules as IPlacementRule[] ?? rules.ToArray();

        _calculationRules = placementRules.OrderBy(pr => pr.Order).ToArray();
    }

    public IEnumerable<PlacementScoreModel> FindBestWord(Board board, List<Letter> lettersOnHand)
    {
        var stopwatch = Stopwatch.StartNew();

        var allPlacements = GenerateAllPlacements(board, lettersOnHand);
        var results = ValidateAndScorePlacements(board, allPlacements);

        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);

        return results;
    }

    private IEnumerable<PlacementScoreModel> ValidateAndScorePlacements(
        Board board,
        IEnumerable<PlacementModel> allPlacements
    )
    {
        var results = new ConcurrentBag<PlacementScoreModel>();

        Parallel.ForEach(
            allPlacements,
            (placement, token) =>
            {
                if (ValidatePlacement(board, placement) is null)
                    return;

                var scoredPlacements = ScoringUtility.ScorePlacement(board, placement);
                results.Add(scoredPlacements);
            }
        );

        return results.OrderByDescending(r => r.Score).Take(10);
    }

    private static IEnumerable<PlacementModel> GenerateAllPlacements(
        Board board,
        List<Letter> lettersOnHand
    )
    {
        return (
            from alignment in BoardConstants.AllAlignments
            from letters in CombinationUtils.GetAllPermutationsOfAllSubsets(lettersOnHand)
            let lettersArray = letters.ToArray()
            from coordinate in board.BoardLetters.SelectMany(bl =>
                BoardUtility.GetAllCoordinatesWithinDistance(bl.Coordinate, lettersArray.Length)
            )
            select new PlacementModel
            {
                Coordinate = coordinate,
                Alignment = alignment,
                Letters = letters
            }
        ).Distinct();
    }

    private PlacementModel? ValidatePlacement(Board board, PlacementModel placement)
    {
        return _calculationRules.Any(rule => !rule.Validate(board, placement)) ? null : placement;
    }
}
