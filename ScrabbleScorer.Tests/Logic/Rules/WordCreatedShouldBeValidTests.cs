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
}