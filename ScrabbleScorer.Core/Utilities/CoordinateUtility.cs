namespace ScrabbleScorer.Core.Utilities;

public static class CoordinateUtility
{
    extension(Coordinate coordinate)
    {
        public Coordinate NextTile(Alignment alignment, int count = 1)
        {
            return alignment == Alignment.Horizontal
                ? coordinate with
                {
                    X = coordinate.X + count,
                }
                : coordinate with
                {
                    Y = coordinate.Y + count,
                };
        }

        public Coordinate PrevTile(Alignment alignment, int count = 1)
        {
            return alignment == Alignment.Horizontal
                ? coordinate with
                {
                    X = coordinate.X - count,
                }
                : coordinate with
                {
                    Y = coordinate.Y - count,
                };
        }
    }
}
