using Microsoft.EntityFrameworkCore;
using ScrabbleScorer.Core.Constants;
using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Extensions;
using ScrabbleScorer.Core.Models;
using ScrabbleScorer.Core.Utilities;
using ScrabbleScorer.Database;

namespace ScrabbleScorer.Services;

public class WordsService : IWordsService
{
    public async Task<string[]> FindPossibleWordsAsync(
        string letters,
        (int position, char letter)[] restrictions
    )
    {
        var sortedLetters = letters.ToSortedLetters();

        var letterCombinations = Enumerable
            .Range(1, letters.Length)
            .SelectMany(r => sortedLetters.DifferentCombinations(r))
            .Distinct()
            .Select(w => new string(w.ToArray()))
            .SelectMany(w =>
            {
                if (!w.Contains('_'))
                    return new[] { w };

                var lettersWithoutBlank = new string(w.Where(l => l != '_').ToArray());
                var numBlanks = w.Count(l => l == '_');

                return lettersWithoutBlank.DifferentPermutationsOfBlank(numBlanks);
            })
            .Distinct()
            .ToArray();

        await using var database = new DatabaseContext();

        var restrictionQueryString =
            restrictions.Length > 0
                ? $"w.Word like '{CreateRestrictionQueryString(restrictions, letters.Length)}' and length(w.Word) = {letters.Length}"
                : "true";

        var query = $"select * from Words w where {restrictionQueryString}";

        var wordCombinationsPredicate = string.Join(
            " OR ",
            from wc in letterCombinations
            select $"w.WordSorted like '{wc}'"
        );

        var matchingWords = await database.Words
            .FromSqlRaw($"{query} AND ({wordCombinationsPredicate})")
            .Select(w => w.Word)
            .ToArrayAsync();

        return matchingWords;
    }

    private string CreateRestrictionQueryString(
        (int position, char letter)[] restrictions,
        int wordLength
    )
    {
        var orderedRestrictions = restrictions.OrderBy(r => r.position).ToArray();

        var firstPosition = orderedRestrictions.First().position;
        var lastPosition = orderedRestrictions.Last().position;

        var lettersCount = lastPosition - firstPosition + 1;

        var queryString = Enumerable
            .Range(firstPosition, lettersCount)
            .Select(r =>
            {
                var hasRestrictionOnPosition = restrictions.Any(re => re.position == r);

                return hasRestrictionOnPosition
                    ? restrictions.First(re => re.position == r).letter
                    : '_';
            })
            .ToArray();

        var startWildcard = firstPosition == 1 ? string.Empty : "%";
        var endWildcard = lastPosition == wordLength ? string.Empty : "%";

        return $"{startWildcard}{new string(queryString)}{endWildcard}";
    }

    public (Coordinate coordinate, Alignment alignment)[] FindPossibleWordLocations(
        Board board,
        int wordLength
    )
    {
        var occupiedCoordinates = board.BoardLetters.Select(bl => bl.Coordinate).ToArray();

        return BoardCoordinateConstants.AllCoordinates
            .Where(
                c =>
                    CoordinateUtility.CanMakeWordOnCoordinate(
                        occupiedCoordinates,
                        c,
                        Alignment.Horizontal,
                        wordLength
                    )
            )
            .Select(c => (c, Alignment.Horizontal))
            .Concat(
                BoardCoordinateConstants.AllCoordinates
                    .Where(
                        c =>
                            CoordinateUtility.CanMakeWordOnCoordinate(
                                occupiedCoordinates,
                                c,
                                Alignment.Vertical,
                                wordLength
                            )
                    )
                    .Select(c => (c, Alignment.Vertical))
            )
            .Distinct()
            .ToArray();
    }

    public async Task<WordPlacementModel[]> FindTopScoringWordsAsync(Board board, string letters)
    {
        var validPossibleWords = new List<WordPlacementModel>();

        foreach (
            var wordLength in Enumerable
                .Range(1, BoardCoordinateConstants.BoardSize - letters.Length + 1)
                .Reverse()
        )
        {
            var possibleLocations = FindPossibleWordLocations(board, wordLength);

            foreach (var location in possibleLocations)
            {
                var placementRestrictions = GetPlacementRestrictions(
                    board,
                    location.coordinate,
                    location.alignment,
                    wordLength
                );

                var possibleWords = await FindPossibleWordsAsync(letters, placementRestrictions);

                foreach (var possibleWord in possibleWords.Where(w => w.Length == wordLength))
                {
                    if (
                        await IsAdjacentWordsValidAsync(
                            board.BoardLetters,
                            location.coordinate,
                            location.alignment,
                            possibleWord
                        )
                    )
                    {
                        validPossibleWords.Add(
                            new WordPlacementModel
                            {
                                Word = possibleWord,
                                Coordinate = location.coordinate,
                                Alignment = location.alignment
                            }
                        );
                    }
                }
            }
        }

        return validPossibleWords.ToArray();
    }

    private BoardLetter[] TryPlaceWord(
        BoardLetter[] occupiedCoordinates,
        Coordinate coordinate,
        Alignment alignment,
        string letters
    )
    {
        var tempBoardLetters = occupiedCoordinates.ToList();

        var currCoordinate = coordinate;

        for (var i = 0; i < letters.Length; i++)
        {
            var letter = letters[i];

            if (tempBoardLetters.All(bl => bl.Coordinate != currCoordinate))
                tempBoardLetters.Add(
                    new BoardLetter { Letter = letter.ToLetter(), Coordinate = currCoordinate }
                );

            if (i != letters.Length - 1)
                currCoordinate = currCoordinate.Next(alignment);
        }

        return tempBoardLetters.ToArray();
    }

    private async Task<bool> IsAdjacentWordsValidAsync(
        BoardLetter[] boardLetters,
        Coordinate coordinate,
        Alignment alignment,
        string word
    )
    {
        var bls = TryPlaceWord(boardLetters, coordinate, alignment, word);
        var boardLetterCoords = bls.Select(bl => bl.Coordinate).ToArray();

        var wordStartCoord = coordinate.FirstNonBlank(boardLetterCoords, alignment);
        var wordEndCoord = coordinate.LastNonBlank(boardLetterCoords, alignment);

        foreach (var coord in wordStartCoord.To(wordEndCoord, alignment))
        {
            var start = coord.FirstNonBlank(boardLetterCoords, alignment.Inverted());
            var end = coord.LastNonBlank(boardLetterCoords, alignment.Inverted());

            if (start == end)
                continue;

            if (!await IsValidWordAsync(bls, start, end, alignment.Inverted()))
                return false;
        }

        return await IsValidWordAsync(bls, wordStartCoord, wordEndCoord, alignment);
    }

    private async Task<bool> IsValidWordAsync(
        BoardLetter[] boardLetters,
        Coordinate start,
        Coordinate end,
        Alignment alignment
    )
    {
        var chars = (
            from coordinate in start.To(end, alignment)
            select boardLetters.First(bl => bl.Coordinate == coordinate).Letter.ToChar()
        ).ToArray();

        var word = new string(chars);

        await using var database = new DatabaseContext();

        return await database.Words.AnyAsync(w => w.Word == word);
    }

    private (int position, char letter)[] GetPlacementRestrictions(
        Board board,
        Coordinate coordinate,
        Alignment alignment,
        int wordLength
    )
    {
        var currCoordinate = coordinate;
        var restrictions = new List<(int, char)>();

        for (var pos = 1; pos <= wordLength; pos++)
        {
            var boardLetter = board.BoardLetters.FirstOrDefault(
                bl => bl.Coordinate == currCoordinate
            );

            if (boardLetter is not null)
            {
                restrictions.Add((pos, boardLetter.Letter.ToChar()));
            }

            if (pos != wordLength)
                currCoordinate = currCoordinate.Next(alignment);
        }

        return restrictions.ToArray();
    }
}
