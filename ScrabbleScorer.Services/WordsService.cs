using System.Collections.Immutable;
using LazyCache;
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
    private readonly IAppCache _appCache;

    public WordsService(IAppCache appCache)
    {
        _appCache = appCache;
    }

    public async Task<string[]> FindPossibleWordsAsync(
        string letters,
        (int position, char letter)[] restrictions,
        int wordLength
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

        var temp = string.Empty;
        if (restrictions.Length > 0)
            temp = CreateRestrictionQueryString(restrictions, letters.Length);

        var restrictionQueryString =
            restrictions.Length > 0
                ? $"w.Word like '{temp}' and length(w.Word) = {letters.Length}"
                : "true";

        return await _appCache.GetOrAddAsync(
            $"{wordLength} {letters} {temp} {letters.Length}",
            async () =>
            {
                await using var database = new DatabaseContext();

                var query =
                    $"select * from Words w where length(w.Word) == {wordLength} and {restrictionQueryString}";

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
        );
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
        int wordLength,
        int lettersOnHandLength
    )
    {
        var occupiedCoordinates = board.BoardLetters.Select(bl => bl.Coordinate).ToHashSet();

        return BoardCoordinateConstants.AllCoordinates
            .Where(
                c =>
                    CoordinateUtility.CanMakeWordOnCoordinate(
                        occupiedCoordinates,
                        c,
                        Alignment.Horizontal,
                        wordLength,
                        lettersOnHandLength
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
                                wordLength,
                                lettersOnHandLength
                            )
                    )
                    .Select(c => (c, Alignment.Vertical))
            )
            .Distinct()
            .ToArray();
    }

    public async Task<TopScoringWordModel[]> FindTopScoringWordsAsync(Board board, string letters)
    {
        var validPossibleWords =
            new List<(string word, Coordinate coordinate, Alignment alignment)>();

        foreach (
            var wordLength in Enumerable.Range(1, BoardCoordinateConstants.BoardSize).Reverse()
        )
        {
            var possibleLocations = FindPossibleWordLocations(board, wordLength, letters.Length);

            foreach (var location in possibleLocations)
            {
                var placementRestrictions = GetPlacementRestrictions(
                    board,
                    location.coordinate,
                    location.alignment,
                    wordLength
                );

                var possibleWords = await FindPossibleWordsAsync(
                    letters,
                    placementRestrictions,
                    wordLength
                );

                foreach (var possibleWord in possibleWords)
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
                            (possibleWord, location.coordinate, location.alignment)
                        );
                    }
                }
            }
        }

        return (
            from x in validPossibleWords
            let score = ScoreUtility.CalculateScore(board, x.coordinate, x.alignment, x.word)
            select new TopScoringWordModel
            {
                Score = score,
                Word = x.word,
                Coordinate = x.coordinate,
                Alignment = x.alignment,
            }
        ).OrderByDescending(x => x.Score).Take(3).ToArray();
    }

    private async Task<bool> IsAdjacentWordsValidAsync(
        BoardLetter[] boardLetters,
        Coordinate coordinate,
        Alignment alignment,
        string word
    )
    {
        var boardLettersAfterPlacement = BoardUtility.TryPlaceWord(
            boardLetters,
            coordinate,
            alignment,
            word
        );
        var boardLetterCoords = boardLettersAfterPlacement
            .Select(bl => bl.Coordinate)
            .ToImmutableHashSet();

        var wordStartCoord = coordinate.FirstNonBlank(boardLetterCoords, alignment);
        var wordEndCoord = coordinate.LastNonBlank(boardLetterCoords, alignment);

        foreach (var coord in wordStartCoord.To(wordEndCoord, alignment))
        {
            var start = coord.FirstNonBlank(boardLetterCoords, alignment.Inverted());
            var end = coord.LastNonBlank(boardLetterCoords, alignment.Inverted());

            if (start == end)
                continue;

            if (
                !await IsValidWordAsync(
                    boardLettersAfterPlacement,
                    start,
                    end,
                    alignment.Inverted()
                )
            )
                return false;
        }

        return await IsValidWordAsync(
            boardLettersAfterPlacement,
            wordStartCoord,
            wordEndCoord,
            alignment
        );
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

        return await _appCache.GetOrAddAsync(
            word,
            async () =>
            {
                await using var database = new DatabaseContext();

                return await database.Words.AnyAsync(w => w.Word == word);
            }
        );
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
