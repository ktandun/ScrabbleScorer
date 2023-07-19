using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Models;
using ScrabbleScorer.Core.Utilities;

namespace ScrabbleScorer.Tests;

public class ScoreUtilityTest
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void TestLetterValue()
    {
        var value = 'J'.ToLetterValue();

        Assert.That(value, Is.EqualTo(8));
    }

    [Test]
    public void TestLetterEnum()
    {
        var value = 'J'.ToLetter();

        Assert.That(value, Is.EqualTo(Letter.J));
    }

    [Test]
    public void TestCalculator()
    {
        var score = ScoreUtility.CalculateScore("QI", new[] { BonusType.None, BonusType.None });

        Assert.That(score, Is.EqualTo(11));
    }

    [Test]
    public void TestCalculatorDoubleLetterBonus()
    {
        var score = ScoreUtility.CalculateScore(
            "QI",
            new[] { BonusType.DoubleLetter, BonusType.None }
        );

        Assert.That(score, Is.EqualTo(21));
    }

    [Test]
    public void TestCalculatorTripleLetterBonus()
    {
        var score = ScoreUtility.CalculateScore(
            "QI",
            new[] { BonusType.TripleLetter, BonusType.None }
        );

        Assert.That(score, Is.EqualTo(31));
    }

    [Test]
    public void TestCalculatorDoubleWordBonus()
    {
        var score = ScoreUtility.CalculateScore(
            "QI",
            new[] { BonusType.DoubleWord, BonusType.None }
        );

        Assert.That(score, Is.EqualTo(22));
    }

    [Test]
    public void TestCalculatorTripleWordBonus()
    {
        var score = ScoreUtility.CalculateScore(
            "QI",
            new[] { BonusType.TripleWord, BonusType.None }
        );

        Assert.That(score, Is.EqualTo(33));
    }

    [Test]
    public void CalculateScoreTest()
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

        var score = ScoreUtility.CalculateScore(
            board,
            new Coordinate(9, 7),
            Alignment.Vertical,
            "OO"
        );
    }
}
