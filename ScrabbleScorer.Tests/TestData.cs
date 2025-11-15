using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Tests;

public static class TestData
{
    public static readonly Board EmptyBoard = new() { BoardLetters = [] };

    public static readonly Board SingleWordBoard = new()
    {
        BoardLetters =
        [
            new LetterOnBoard { Letter = Letter.C, Coordinate = new Coordinate(8, 8) },
            new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(9, 8) },
            new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(10, 8) },
        ],
    };

    public static readonly Board TwoUnconnectedWordBoard = new()
    {
        BoardLetters =
        [
            new LetterOnBoard { Letter = Letter.C, Coordinate = new Coordinate(8, 8) },
            new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(9, 8) },
            new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(10, 8) },
            //
            new LetterOnBoard { Letter = Letter.B, Coordinate = new Coordinate(12, 8) },
            new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(13, 8) },
            new LetterOnBoard { Letter = Letter.D, Coordinate = new Coordinate(14, 8) },
        ],
    };
}
