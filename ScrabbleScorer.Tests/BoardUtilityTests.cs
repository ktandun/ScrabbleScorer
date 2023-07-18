using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Models;
using ScrabbleScorer.Core.Utilities;

namespace ScrabbleScorer.Tests;

public class BoardUtilityTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void TestLetterValue()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(8, 7) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(8, 6) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 5) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(8, 4) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 3) },
                new() { Letter = Letter.P, Coordinate = new Coordinate(9, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(10, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(11, 8) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(12, 8) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(10, 7) },
                new() { Letter = Letter.H, Coordinate = new Coordinate(10, 6) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(10, 5) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(10, 4) },
            }
        };

        var words = BoardUtility.GetWordsOnBoard(board.BoardLetters);
    }

    [Test]
    public void FindCreatedWordsAfterPlacementTest()
    {
        var before = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(8, 7) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(8, 6) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 5) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(8, 4) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 3) },
            }
        };

        var after = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(8, 7) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(8, 6) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 5) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(8, 4) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 3) },
                new() { Letter = Letter.P, Coordinate = new Coordinate(9, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(10, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(11, 8) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(12, 8) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(10, 7) },
                new() { Letter = Letter.H, Coordinate = new Coordinate(10, 6) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(10, 5) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(10, 4) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(9, 7) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(9, 6) },
            }
        };

        var words = BoardUtility.FindCreatedWordsAfterPlacement(
            before.BoardLetters,
            after.BoardLetters
        );

        Assert.Multiple(() =>
        {
            Assert.That(
                words.Contains(
                    new WordPlacementModel
                    {
                        Word = "SPOON",
                        Alignment = Alignment.Horizontal,
                        Coordinate = new Coordinate(8, 8)
                    }
                ),
                Is.True
            );

            Assert.That(
                words.Contains(
                    new WordPlacementModel
                    {
                        Word = "OTHER",
                        Alignment = Alignment.Vertical,
                        Coordinate = new Coordinate(10, 8)
                    }
                ),
                Is.True
            );

            Assert.That(
                words.Contains(
                    new WordPlacementModel
                    {
                        Word = "POO",
                        Alignment = Alignment.Vertical,
                        Coordinate = new Coordinate(9, 8)
                    }
                ),
                Is.True
            );

            Assert.That(
                words.Contains(
                    new WordPlacementModel
                    {
                        Word = "OOT",
                        Alignment = Alignment.Horizontal,
                        Coordinate = new Coordinate(8, 7)
                    }
                ),
                Is.True
            );

            Assert.That(
                words.Contains(
                    new WordPlacementModel
                    {
                        Word = "NOH",
                        Alignment = Alignment.Horizontal,
                        Coordinate = new Coordinate(8, 6)
                    }
                ),
                Is.True
            );
        });
    }
}
