using System.Collections.Immutable;
using ScrabbleScorer.Core.Constants;
using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Extensions;
using ScrabbleScorer.Core.Models;

namespace ScrabbleScorer.Core.Utilities;

public static class BoardUtility
{
    public static (BoardLetter[], bool isBingo) TryPlaceWord(
        BoardLetter[] occupiedCoordinates,
        Coordinate coordinate,
        Alignment alignment,
        string letters
    )
    {
        var tempBoardLetters = occupiedCoordinates.ToList();

        var currCoordinate = coordinate;
        var lettersPutDown = 0;

        for (var i = 0; i < letters.Length; i++)
        {
            var letter = letters[i];

            if (tempBoardLetters.All(bl => bl.Coordinate != currCoordinate))
            {
                tempBoardLetters.Add(
                    new BoardLetter { Letter = letter.ToLetter(), Coordinate = currCoordinate }
                );
                
                lettersPutDown++;
            }

            if (i != letters.Length - 1)
                currCoordinate = currCoordinate.Next(alignment);
        }

        var isBingo = lettersPutDown == 7;

        return (tempBoardLetters.ToArray(), isBingo);
    }

    public static WordPlacementModel[] GetWordsOnBoard(BoardLetter[] boardLetters)
    {
        var occupiedCoordinates = boardLetters.Select(bl => bl.Coordinate).ToImmutableHashSet();
        var alignments = new[] { Alignment.Horizontal, Alignment.Vertical };

        return (
            from boardCoord in BoardCoordinateConstants.AllCoordinates
            from alignment in alignments
            where occupiedCoordinates.Contains(boardCoord)
            where
                boardCoord.Next(alignment, peek: true).IsWithinBoardDimensions()
                && occupiedCoordinates.Contains(boardCoord.Next(alignment))
                && !occupiedCoordinates.Contains(boardCoord.Prev(alignment, peek: true))
            let wordFirstCoordinate = boardCoord
            let wordLastCoordinate = boardCoord.LastNonBlank(occupiedCoordinates, alignment)
            let wordCoordinates = wordFirstCoordinate.To(wordLastCoordinate, alignment).ToArray()
            let word = GetWordFromCoordinates(boardLetters, wordCoordinates)
            select new WordPlacementModel
            {
                Word = word,
                Alignment = alignment,
                StartCoordinate = boardCoord,
                Coordinates = wordCoordinates
            }
        ).ToArray();
    }

    private static string GetWordFromCoordinates(
        BoardLetter[] boardLetters,
        IEnumerable<Coordinate> coordinates
    )
    {
        return new string(
            (
                from coordinate in coordinates
                let boardLetter = boardLetters.First(bl => bl.Coordinate == coordinate)
                select boardLetter.Letter.ToChar()
            ).ToArray()
        );
    }

    public static WordPlacementModel[] FindCreatedWordsAfterPlacement(
        BoardLetter[] before,
        BoardLetter[] after
    )
    {
        var wordsBefore = GetWordsOnBoard(before);
        var wordsAfter = GetWordsOnBoard(after);

        var createdWords = wordsAfter.Where(
            wa =>
                !wordsBefore.Any(
                    wb => wb.StartCoordinate == wa.StartCoordinate && wb.Alignment == wa.Alignment
                )
        );

        return createdWords.ToArray();
    }
}
