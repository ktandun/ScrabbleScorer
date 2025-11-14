using System.Collections.Immutable;

namespace ScrabbleScorer.Core.Constants;

public static class BoardCoordinateConstants
{
    public const int BoardSize = 15;
    public static readonly Coordinate CentreTile = new(8, 8);

    /**
     * 1,1...........15,1
     * .                .
     * .                .
     * .                .
     * .                .
     * .                .
     * 1,15..........15,15
     */
    public static readonly Coordinate[] AllCoordinates =
    [
        .. from y in Enumerable.Range(1, BoardSize)
        from x in Enumerable.Range(1, BoardSize)
        select new Coordinate(x, y)
    ];

    public static readonly Alignment[] AllAlignments = [Alignment.Horizontal, Alignment.Vertical];

    public static readonly BonusTile[] BonusTiles =
    {
        new() { Coordinate = new Coordinate(1, 1), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(8, 1), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(15, 1), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(1, 8), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(15, 8), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(1, 15), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(8, 15), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(15, 15), BonusType = BonusType.TW },
        new() { Coordinate = new Coordinate(6, 2), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(10, 2), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(2, 6), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(6, 6), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(10, 6), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(13, 6), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(6, 14), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(10, 14), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(2, 10), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(6, 10), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(10, 10), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(13, 10), BonusType = BonusType.TL },
        new() { Coordinate = new Coordinate(4, 1), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(12, 1), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(7, 3), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(9, 3), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(1, 4), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(8, 4), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(15, 4), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(3, 7), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(7, 7), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(9, 7), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(13, 7), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(4, 8), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(12, 8), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(9, 9), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(13, 9), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(7, 9), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(3, 9), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(15, 12), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(8, 12), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(1, 12), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(9, 13), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(7, 13), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(12, 15), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(4, 15), BonusType = BonusType.DL },
        new() { Coordinate = new Coordinate(2, 2), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(14, 2), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(3, 3), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(13, 3), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(4, 4), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(12, 4), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(5, 5), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(11, 5), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(8, 8), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(2, 14), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(14, 14), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(3, 13), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(13, 13), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(4, 12), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(12, 12), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(5, 11), BonusType = BonusType.DW },
        new() { Coordinate = new Coordinate(11, 11), BonusType = BonusType.DW }
    };
}
