using System.Collections.Concurrent;
using System.Diagnostics;
using ScrabbleScorer.Core.Logic.Rules;

namespace ScrabbleScorer.Core.Services;

public interface IGameService
{
    List<PlacementScoreModel> FindBestWord(Board board, List<Letter> lettersOnHand);
}

public class GameService : IGameService
{
    private readonly IPlacementRule[] _calculationRules;

    public GameService(IEnumerable<IPlacementRule> rules)
    {
        var placementRules = rules as IPlacementRule[] ?? rules.ToArray();

        _calculationRules = placementRules.OrderBy(pr => pr.Order).ToArray();
    }

    public List<PlacementScoreModel> FindBestWord(Board board, List<Letter> lettersOnHand)
    {
        var stopwatch = Stopwatch.StartNew();

        var allPlacements = GenerateAllPlacements(board, lettersOnHand).ToArray();
        var results = ValidatePlacements(board, allPlacements);
        var scoredPlacements = ScorePlacements(board, results);
        var highestNScores = scoredPlacements.OrderByDescending(sp => sp.Score).Take(10).ToList();

        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);

        return highestNScores;
    }

    private List<PlacementModel> ValidatePlacements(
        Board board,
        IEnumerable<PlacementModel> allPlacements
    )
    {
        var validPlacements = new ConcurrentBag<PlacementModel>();

        Parallel.ForEach(
            allPlacements,
            new ParallelOptions() { MaxDegreeOfParallelism = 4 },
            (placement, token) =>
            {
                if (ValidatePlacement(board, placement) is not null)
                    validPlacements.Add(placement);
            }
        );

        var results = validPlacements.ToList();
        return results;
    }

    private List<PlacementScoreModel> ScorePlacements(Board board, List<PlacementModel> placements)
    {
        return placements.Select(p => ScoringUtility.ScorePlacement(board, p)).Distinct().ToList();
    }

    private IEnumerable<PlacementModel> GenerateAllPlacements(
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
        );
    }

    private PlacementModel? ValidatePlacement(Board board, PlacementModel placement)
    {
        return _calculationRules.Any(rule => !rule.Validate(board, placement)) ? null : placement;
    }
}
