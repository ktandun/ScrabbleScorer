using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Models;
using ScrabbleScorer.Services;
using ScrabbleScorer.Tests.Utilities;

namespace ScrabbleScorer.Tests;

public class WordsServiceTest
{
    [Test]
    public async Task FindPossibleWordsTest()
    {
        var wordsService = new WordsService();

        var words = await wordsService.FindPossibleWordsAsync("quebech", Array.Empty<(int, char)>());
        
        Assert.That(words, Is.Not.Empty);
    }
    
    [Test]
    public async Task FindPossibleWordsTest_WithBlankLetter()
    {
        var wordsService = new WordsService();

        var words = await wordsService.FindPossibleWordsAsync("ca_", Array.Empty<(int, char)>());
        
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
        var wordsService = new WordsService();

        var words = await wordsService.FindPossibleWordsAsync("atanos", new[]
        {
            (1, 's'),
            (3, 'n'),
            (5, 't')
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(words.Length, Is.EqualTo(1));
            
            Assert.That(words.Contains("SONATA"), Is.True);
        });
    }
    
    [Test]
    public async Task FindPossibleWordsTest_WithRestrictions()
    {
        var wordsService = new WordsService();

        var words = await wordsService.FindPossibleWordsAsync("ca_", new[]
        {
            (1, 'a')
        });
        
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
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(84));
    }
    
    [Test]
    public void FindPossibleWordLocationsTest_BottomOfBoard()
    {
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(30));
        
        DebugUtility.DrawInHtml(board.BoardLetters.Select(bl => bl.Coordinate).ToArray(), possibleLocations, 6);
    }
    
    [Test]
    public void FindPossibleWordLocationsTest_TopOfBoard()
    {
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(30));
    }
    
    [Test]
    public void FindPossibleWordLocationsTest_TopLeftCornerOfBoard()
    {
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));
    }
    
    [Test]
    public void FindPossibleWordLocationsTest_TopRightCornerOfBoard()
    {
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));
    }
    
    [Test]
    public void FindPossibleWordLocationsTest_BottomRightCornerOfBoard()
    {
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));
        
        DebugUtility.DrawInHtml(board.BoardLetters.Select(bl => bl.Coordinate).ToArray(), possibleLocations, 6);
    }
    
    [Test]
    public void FindPossibleWordLocationsTest_BottomLeftCornerOfBoard()
    {
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(25));
    }
    
    [Test]
    public void FindPossibleWordLocationsTest_MiddleOfBoard_VerticalWord()
    {
        var wordsService = new WordsService();

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
        
        var possibleLocations = wordsService.FindPossibleWordLocations(board, 6);

        Assert.That(possibleLocations.Length, Is.EqualTo(84));
    }
    
    [TestCase(1)] [TestCase(2)] [TestCase(3)] 
    [TestCase(4)] [TestCase(5)] [TestCase(6)] 
    [TestCase(7)] [TestCase(8)] [TestCase(9)] 
    [TestCase(10)] [TestCase(11)] [TestCase(12)] 
    [TestCase(13)] [TestCase(14)] [TestCase(15)]
    public void FindPossibleWordLocationsTest_MiddleOfBoard_MultipleWords(int wordLength)
    {
        var wordsService = new WordsService();

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

        var possibleLocations = wordsService.FindPossibleWordLocations(board, wordLength);

        DebugUtility.DrawInHtml(board.BoardLetters.Select(bl => bl.Coordinate).ToArray(), possibleLocations, wordLength);
    }
    
    [TestCase(1)] [TestCase(2)] [TestCase(3)] 
    [TestCase(4)] [TestCase(5)] [TestCase(6)] 
    [TestCase(7)] [TestCase(8)] [TestCase(9)] 
    [TestCase(10)] [TestCase(11)] [TestCase(12)] 
    [TestCase(13)] [TestCase(14)] [TestCase(15)]
    public void FindPossibleWordLocationsTest_MiddleOfBoard_MultipleStackingWords(int wordLength)
    {
        var wordsService = new WordsService();

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

        var possibleLocations = wordsService.FindPossibleWordLocations(board, wordLength);

        DebugUtility.DrawInHtml(board.BoardLetters.Select(bl => bl.Coordinate).ToArray(), possibleLocations, wordLength);
    }
    
    [Test]
    public async Task FindTopScoringWordsAsyncTest()
    {
        var wordsService = new WordsService();

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

        var wordPlacementModels = await wordsService.FindTopScoringWordsAsync(board, "hellowo");
    }
}