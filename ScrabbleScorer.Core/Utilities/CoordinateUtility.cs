namespace ScrabbleScorer.Core.Utilities;

public static class CoordinateUtility
{
    public static Coordinate NextTile(
        this Coordinate coordinate,
        Alignment alignment,
        int count = 1
    )
    {
        return alignment == Alignment.Horizontal
            ? coordinate with
            {
                X = coordinate.X + count
            }
            : coordinate with
            {
                Y = coordinate.Y + count
            };
    }

    public static Coordinate PrevTile(
        this Coordinate coordinate,
        Alignment alignment,
        int count = 1
    )
    {
        return alignment == Alignment.Horizontal
            ? coordinate with
            {
                X = coordinate.X - count
            }
            : coordinate with
            {
                Y = coordinate.Y - count
            };
    }
}
