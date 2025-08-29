using Microsoft.Extensions.Caching.Memory;
using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Logic.Rules;
using ScrabbleScorer.Core.Repositories;

namespace ScrabbleScorer.Tests.Logic.Rules;

public class WordCreatedShouldBeValidTests
{
    private readonly WordCreatedShouldBeValid _sut;

    public WordCreatedShouldBeValidTests()
    {
        var wordRepository = new WordRepository(new MemoryCache(new MemoryCacheOptions()));
        _sut = new WordCreatedShouldBeValid(wordRepository);
    }

    [Fact]
    public async Task NormalWord()
    {
        var valid = await _sut.ValidateAsync(
            new Board
            {
                BoardLetters = []
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(1, 1),
                Alignment = Alignment.Horizontal,
                Letters =
                [
                    Letter.C,
                    Letter.A,
                    Letter.T
                ]
            });

        Assert.True(valid);
    }

    [Fact]
    public async Task WordWithBlank()
    {
        var valid = await _sut.ValidateAsync(
            new Board
            {
                BoardLetters = []
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(1, 1),
                Alignment = Alignment.Horizontal,
                Letters =
                [
                    Letter.C,
                    Letter.Blank,
                    Letter.T
                ]
            });

        Assert.True(valid);
    }

    [Theory]
    [InlineData(Alignment.Horizontal)]
    [InlineData(Alignment.Vertical)]
    public async Task PlacingWordWithExistingWordOnOneSide(Alignment alignment)
    {
        var valid = await _sut.ValidateAsync(
            new Board
            {
                BoardLetters =
                [
                    new BoardLetter
                    {
                        Letter = Letter.C,
                        Coordinate = new Coordinate(3, 1),
                    },
                    new BoardLetter
                    {
                        Letter = Letter.A,
                        Coordinate = new Coordinate(3, 2),
                    },
                    new BoardLetter
                    {
                        Letter = Letter.C,
                        Coordinate = new Coordinate(1, 3),
                    },
                    new BoardLetter
                    {
                        Letter = Letter.A,
                        Coordinate = new Coordinate(2, 3),
                    }
                ]
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(3, 3),
                Alignment = alignment,
                Letters =
                [
                    Letter.B,
                    Letter.L,
                    Letter.E
                ]
            });

        Assert.True(valid);
    }
    
    [Fact]
    public async Task PlacingWordWithExistingWordOnBothSides()
    {
        var valid = await _sut.ValidateAsync(
            new Board
            {
                BoardLetters =
                [
                    new BoardLetter
                    {
                        Letter = Letter.C,
                        Coordinate = new Coordinate(1, 1),
                    },
                    new BoardLetter
                    {
                        Letter = Letter.A,
                        Coordinate = new Coordinate(1, 2),
                    },
                    new BoardLetter
                    {
                        Letter = Letter.T,
                        Coordinate = new Coordinate(1, 4),
                    },
                ]
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(1, 3),
                Alignment = Alignment.Vertical,
                Letters =
                [
                    Letter.S,
                ]
            });

        Assert.True(valid);
    }
}