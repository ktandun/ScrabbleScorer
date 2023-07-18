using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public record Board()
{
    public required BoardLetter[] BoardLetters { get; init; }
}
