using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Logic.Rules;
using ScrabbleScorer.Core.Utilities;

namespace ScrabbleScorer.Tests.Utilities;

public class BoardUtilityTests
{
    [Fact]
    public void TryPlaceLetters()
    {
        var board = TestData.TwoUnconnectedWordBoard;

        var (_, finalCoordinate, wordCreated) = board.TryPlaceLetters(
            new PlacementModel
            {
                Coordinate = new Coordinate(7, 8),
                Alignment = Alignment.Horizontal,
                Letters = [Letter.X, Letter.Y, Letter.Z],
            }
        );

        Assert.Equal(new Coordinate(15, 8), finalCoordinate);
        Assert.Equal("XCATYBEDZ", wordCreated.ToWord());
    }

    [Fact]
    public void GetAllCoordinatesWithinDistance()
    {
        var coord = new Coordinate(10, 14);

        var results = BoardUtility.GetAllCoordinatesWithinDistance(coord, 6);
    }
}
