using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Core.Models;

public class WordPlacementModel
{
    public required string Word { get; set; }
    public required Alignment Alignment { get; set; }
    public required Coordinate Coordinate { get; set; }
}