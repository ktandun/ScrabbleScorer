using System.Diagnostics;
using LazyCache;
using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Models;
using ScrabbleScorer.Services;
using ScrabbleScorer.Tests.Utilities;

namespace ScrabbleScorer.Tests;

public class WordsServiceTest
{
    private WordsService _wordsService;

    [SetUp]
    public void Setup()
    {
        _wordsService = new WordsService(new CachingService());
    }

    [Test]
    public async Task FindPossibleWordsTest()
    {
        var words = await _wordsService.FindPossibleWordsAsync(
            "quebech",
            Array.Empty<(int, char)>(),
            2
        );

        Assert.That(words, Is.Not.Empty);
    }

    [Test]
    public async Task FindPossibleWordsTest_WithBlankLetter()
    {
        var words = await _wordsService.FindPossibleWordsAsync(
            "ca_",
            Array.Empty<(int, char)>(),
            3
        );

        Assert.Multiple(() =>
        {
            Assert.That(words.Contains("CAB"), Is.True);
            Assert.That(words.Contains("CAT"), Is.True);
            Assert.That(words.Contains("ACT"), Is.True);
        });
    }

    [Test]
    public async Task FindPossibleWordsTest_WithRestrictions2()
    {
        var words = await _wordsService.FindPossibleWordsAsync(
            "atanos",
            new[] { (1, 's'), (3, 'n'), (5, 't') },
            6
        );

        Assert.Multiple(() =>
        {
            Assert.That(words.Length, Is.EqualTo(1));

            Assert.That(words.Contains("SONATA"), Is.True);
        });
    }

    [Test]
    public async Task FindPossibleWordsTest_WithRestrictions()
    {
        var words = await _wordsService.FindPossibleWordsAsync("ca_", new[] { (1, 'a') }, 3);

        Assert.Multiple(() =>
        {
            Assert.That(words.Length, Is.EqualTo(3));

            Assert.That(words.Contains("ACT"), Is.True);
            Assert.That(words.Contains("ACE"), Is.True);
            Assert.That(words.Contains("ARC"), Is.True);
        });
    }

    [Test]
    public void FindPossibleWordLocationsTest_MiddleOfBoard()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(9, 8) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(10, 8) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(11, 8) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(12, 8) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(13, 8) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(84));
    }

    [Test]
    public void FindPossibleWordLocationsTest_BottomOfBoard()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(8, 1) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(9, 1) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(10, 1) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(11, 1) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(12, 1) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(13, 1) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(30));

        DebugUtility.DrawInHtml(
            board.BoardLetters.Select(bl => bl.Coordinate).ToArray(),
            possibleLocations,
            6
        );
    }

    [Test]
    public void FindPossibleWordLocationsTest_TopOfBoard()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(8, 15) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(9, 15) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(10, 15) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(11, 15) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(12, 15) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(13, 15) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(30));
    }

    [Test]
    public void FindPossibleWordLocationsTest_TopLeftCornerOfBoard()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(1, 15) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(2, 15) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(3, 15) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(4, 15) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(5, 15) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(6, 15) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));
    }

    [Test]
    public void FindPossibleWordLocationsTest_TopRightCornerOfBoard()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(10, 15) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(11, 15) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(12, 15) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(13, 15) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(14, 15) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(15, 15) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));
    }

    [Test]
    public void FindPossibleWordLocationsTest_BottomRightCornerOfBoard()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(10, 1) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(11, 1) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(12, 1) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(13, 1) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(14, 1) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(15, 1) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));

        DebugUtility.DrawInHtml(
            board.BoardLetters.Select(bl => bl.Coordinate).ToArray(),
            possibleLocations,
            6
        );
    }

    [Test]
    public void FindPossibleWordLocationsTest_BottomLeftCornerOfBoard()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(1, 1) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(2, 1) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(3, 1) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(4, 1) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(5, 1) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(6, 1) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));
    }

    [Test]
    public void FindPossibleWordLocationsTest_MiddleOfBoard_VerticalWord()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.S, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(8, 9) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(8, 10) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 11) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(8, 12) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 13) },
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        Assert.That(possibleLocations.Length, Is.EqualTo(84));
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(11)]
    [TestCase(12)]
    [TestCase(13)]
    [TestCase(14)]
    [TestCase(15)]
    public void FindPossibleWordLocationsTest_MiddleOfBoard_MultipleWords(int wordLength)
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
            }
        };

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        DebugUtility.DrawInHtml(
            board.BoardLetters.Select(bl => bl.Coordinate).ToArray(),
            possibleLocations,
            wordLength
        );
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(11)]
    [TestCase(12)]
    [TestCase(13)]
    [TestCase(14)]
    [TestCase(15)]
    public void FindPossibleWordLocationsTest_MiddleOfBoard_MultipleStackingWords(int wordLength)
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

        var possibleLocations = _wordsService.FindPossibleWordLocations(board, 6, 7);

        DebugUtility.DrawInHtml(
            board.BoardLetters.Select(bl => bl.Coordinate).ToArray(),
            possibleLocations,
            wordLength
        );
    }

    [Test]
    public async Task FindTopScoringWordsAsyncTest()
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

        var wordPlacementModels = await _wordsService.FindTopScoringWordsAsync(board, "poo");
    }

    [Test]
    public async Task RealLifeTest()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.E, Coordinate = new Coordinate(8, 15) },
                new() { Letter = Letter.V, Coordinate = new Coordinate(9, 15) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(10, 15) },
                new() { Letter = Letter.Q, Coordinate = new Coordinate(6, 14) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(7, 14) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(8, 14) },
                new() { Letter = Letter.H, Coordinate = new Coordinate(10, 14) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(11, 14) },
                new() { Letter = Letter.B, Coordinate = new Coordinate(12, 14) },
                new() { Letter = Letter.L, Coordinate = new Coordinate(13, 14) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(14, 14) },
                new() { Letter = Letter.P, Coordinate = new Coordinate(5, 13) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(6, 13) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(7, 13) },
                new() { Letter = Letter.D, Coordinate = new Coordinate(9, 13) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(10, 13) },
                new() { Letter = Letter.G, Coordinate = new Coordinate(11, 13) },
                new() { Letter = Letter.W, Coordinate = new Coordinate(4, 12) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(5, 12) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(6, 12) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(11, 12) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(11, 11) },
                new() { Letter = Letter.D, Coordinate = new Coordinate(11, 10) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(11, 9) },
                new() { Letter = Letter.S, Coordinate = new Coordinate(11, 8) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(6, 8) },
                new() { Letter = Letter.L, Coordinate = new Coordinate(7, 8) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(9, 8) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(10, 8) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(6, 7) },
                new() { Letter = Letter.F, Coordinate = new Coordinate(6, 6) },
            }
        };

        var stopwatch = new Stopwatch();

        stopwatch.Start();
        var wordPlacementModels = await _wordsService.FindTopScoringWordsAsync(board, "ytgctud");
        stopwatch.Stop();

        Assert.That(wordPlacementModels.First().Word, Is.EqualTo("DUCT"));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5_000));
    }

    [Test]
    public async Task RealLifeTest2()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.A, Coordinate = new Coordinate(7, 8) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.J, Coordinate = new Coordinate(7, 9) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(7, 7) },
            }
        };

        var wordPlacementModels = await _wordsService.FindTopScoringWordsAsync(board, "uisexvu");

        Assert.That(wordPlacementModels.First().Word, Is.EqualTo("DUCT"));
    }

    [Test]
    public async Task RealLifeTest3()
    {
        var board = new Board
        {
            BoardLetters = new BoardLetter[]
            {
                new() { Letter = Letter.D, Coordinate = new Coordinate(5, 8) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(6, 8) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(7, 8) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(8, 8) },
                new() { Letter = Letter.H, Coordinate = new Coordinate(6, 9) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(7, 9) },
                new() { Letter = Letter.W, Coordinate = new Coordinate(8, 9) },
                new() { Letter = Letter.F, Coordinate = new Coordinate(9, 9) },
                new() { Letter = Letter.U, Coordinate = new Coordinate(7, 12) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(7, 11) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(7, 10) },
                new() { Letter = Letter.V, Coordinate = new Coordinate(5, 11) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(6, 11) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(8, 15) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 14) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(8, 13) },
                new() { Letter = Letter.H, Coordinate = new Coordinate(8, 12) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(9, 10) },
                new() { Letter = Letter.V, Coordinate = new Coordinate(10, 10) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(11, 10) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(12, 10) },
                new() { Letter = Letter.D, Coordinate = new Coordinate(13, 10) },
                new() { Letter = Letter.L, Coordinate = new Coordinate(11, 11) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(11, 9) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(11, 8) },
                new() { Letter = Letter.B, Coordinate = new Coordinate(10, 12) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(11, 12) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(12, 12) },
                new() { Letter = Letter.M, Coordinate = new Coordinate(13, 12) },
                new() { Letter = Letter.B, Coordinate = new Coordinate(10, 8) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(10, 7) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(10, 6) },
                new() { Letter = Letter.L, Coordinate = new Coordinate(12, 8) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(12, 7) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(12, 6) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(12, 5) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(12, 4) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(12, 3) },
                new() { Letter = Letter.S, Coordinate = new Coordinate(13, 14) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(13, 13) },
                new() { Letter = Letter.J, Coordinate = new Coordinate(5, 5) },
                new() { Letter = Letter.U, Coordinate = new Coordinate(6, 5) },
                new() { Letter = Letter.G, Coordinate = new Coordinate(7, 5) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(8, 5) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(9, 5) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(10, 5) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(10, 14) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(11, 14) },
                new() { Letter = Letter.L, Coordinate = new Coordinate(12, 14) },
                new() { Letter = Letter.S, Coordinate = new Coordinate(12, 2) },
                new() { Letter = Letter.K, Coordinate = new Coordinate(13, 2) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(14, 2) },
                new() { Letter = Letter.Y, Coordinate = new Coordinate(15, 2) },
                new() { Letter = Letter.C, Coordinate = new Coordinate(5, 14) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(6, 14) },
                new() { Letter = Letter.C, Coordinate = new Coordinate(7, 14) },
                new() { Letter = Letter.D, Coordinate = new Coordinate(8, 4) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(8, 3) },
                new() { Letter = Letter.W, Coordinate = new Coordinate(8, 2) },
                new() { Letter = Letter.N, Coordinate = new Coordinate(8, 1) },
                new() { Letter = Letter.P, Coordinate = new Coordinate(5, 1) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(6, 1) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(7, 1) },
                new() { Letter = Letter.U, Coordinate = new Coordinate(9, 1) },
                new() { Letter = Letter.P, Coordinate = new Coordinate(10, 1) },
                new() { Letter = Letter.S, Coordinate = new Coordinate(13, 8) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(13, 7) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(13, 6) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(13, 5) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(13, 4) },
                new() { Letter = Letter.Y, Coordinate = new Coordinate(6, 3) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(6, 2) },
                new() { Letter = Letter.Z, Coordinate = new Coordinate(2, 4) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(3, 4) },
                new() { Letter = Letter.R, Coordinate = new Coordinate(4, 4) },
                new() { Letter = Letter.O, Coordinate = new Coordinate(5, 4) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(14, 4) },
                new() { Letter = Letter.I, Coordinate = new Coordinate(14, 3) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(2, 3) },
                new() { Letter = Letter.X, Coordinate = new Coordinate(3, 3) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(4, 3) },
                new() { Letter = Letter.W, Coordinate = new Coordinate(6, 15) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(6, 13) },
                new() { Letter = Letter.A, Coordinate = new Coordinate(13, 1) },
                new() { Letter = Letter.T, Coordinate = new Coordinate(14, 1) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(15, 1) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(5, 12) },
                new() { Letter = Letter.E, Coordinate = new Coordinate(5, 10) },
            }
        };

        var wordPlacementModels = await _wordsService.FindTopScoringWordsAsync(board, "lmsfdon");

        Assert.That(wordPlacementModels.First().Word, Is.EqualTo("DUCT"));
    }
}
