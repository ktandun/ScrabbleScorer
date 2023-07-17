using ScrabbleScorer.Core.Models;

namespace ScrabbleScorer.Core.Constants;

public static class BoardCoordinateConstants
{
    public const int BoardSize = 15;

    public static readonly Coordinate[] AllCoordinates = (
        from y in Enumerable.Range(1, BoardSize)
        from x in Enumerable.Range(1, BoardSize)
        select new Coordinate(x, y)
    ).ToArray();
}
