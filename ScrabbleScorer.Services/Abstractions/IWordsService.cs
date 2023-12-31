using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Models;

namespace ScrabbleScorer.Services;

public interface IWordsService
{
    (Coordinate coordinate, Alignment alignment)[] FindPossibleWordLocations(
        Board board,
        int wordLength,
        int lettersOnHandLength
    );

    Task<string[]> FindPossibleWordsAsync(
        string letters,
        (int position, char letter)[] restrictions,
        int wordLength
    );
}
