using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public class PlayerHand
{
    public required Letter[] Letters { get; set; }
}