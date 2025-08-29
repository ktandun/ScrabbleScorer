namespace ScrabbleScorer.Core.Enums;

public enum Alignment
{
    Horizontal = 1,
    Vertical = 2
}

public static class AlignmentExtensions
{
    public static Alignment Opposite(this Alignment alignment)
    {
        return alignment == Alignment.Horizontal ? Alignment.Vertical : Alignment.Horizontal;
    }
}