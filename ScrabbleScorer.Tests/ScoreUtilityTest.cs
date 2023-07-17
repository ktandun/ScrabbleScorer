using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Utilities;

namespace ScrabbleScorer.Tests;

public class ScoreUtilityTest
{
    [SetUp]
    public void Setup()
    {
    }

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
            new[] { BonusType.DoubleLetter, BonusType.None });

        Assert.That(score, Is.EqualTo(21));
    }

    [Test]
    public void TestCalculatorTripleLetterBonus()
    {
        var score = ScoreUtility.CalculateScore(
            "QI",
            new[] { BonusType.TripleLetter, BonusType.None });

        Assert.That(score, Is.EqualTo(31));
    }

    [Test]
    public void TestCalculatorDoubleWordBonus()
    {
        var score = ScoreUtility.CalculateScore(
            "QI",
            new[] { BonusType.DoubleWord, BonusType.None });

        Assert.That(score, Is.EqualTo(22));
    }

    [Test]
    public void TestCalculatorTripleWordBonus()
    {
        var score = ScoreUtility.CalculateScore(
            "QI",
            new[] { BonusType.TripleWord, BonusType.None });

        Assert.That(score, Is.EqualTo(33));
    }
}