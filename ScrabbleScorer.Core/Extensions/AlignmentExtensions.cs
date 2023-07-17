using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Extensions;

public static class AlignmentExtensions
{
    public static Alignment Inverted(this Alignment alignment) =>
        alignment == Alignment.Vertical ? Alignment.Horizontal : Alignment.Vertical;
}
