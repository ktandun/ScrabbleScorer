using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Logic.Rules;
using ScrabbleScorer.Core.Repositories;

namespace ScrabbleScorer.Tests.Logic.Rules;

public class WordsCreatedShouldBeValidTests
{
    private readonly WordsCreatedShouldBeValid _sut;

    public WordsCreatedShouldBeValidTests()
    {
        var wordRepository = new WordRepository();
        _sut = new WordsCreatedShouldBeValid(wordRepository);
    }

    [Fact]
    public async Task NormalWord()
    {
        var valid = await _sut.Validate(
            new Board { BoardLetters = [] },
            new PlacementModel
            {
                Coordinate = new Coordinate(1, 1),
                Alignment = Alignment.Horizontal,
                Letters = [Letter.C, Letter.A, Letter.T],
            }
        );

        Assert.True(valid);
    }

    [Fact]
    public async Task WordWithBlank()
    {
        var valid = await _sut.Validate(
            new Board { BoardLetters = [] },
            new PlacementModel
            {
                Coordinate = new Coordinate(1, 1),
                Alignment = Alignment.Horizontal,
                Letters = [Letter.C, Letter.Blank, Letter.T],
            }
        );

        Assert.True(valid);
    }

    [Theory]
    [InlineData(Alignment.Horizontal)]
    [InlineData(Alignment.Vertical)]
    public async Task PlacingWordWithExistingWordOnOneSide(Alignment alignment)
    {
        var valid = await _sut.Validate(
            new Board
            {
                BoardLetters =
                [
                    new LetterOnBoard { Letter = Letter.C, Coordinate = new Coordinate(3, 1) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(3, 2) },
                    new LetterOnBoard { Letter = Letter.C, Coordinate = new Coordinate(1, 3) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(2, 3) },
                ],
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(3, 3),
                Alignment = alignment,
                Letters = [Letter.B, Letter.L, Letter.E],
            }
        );

        Assert.True(valid);
    }

    [Fact]
    public async Task PlacingWordWithExistingWordOnBothSides()
    {
        var valid = await _sut.Validate(
            new Board
            {
                BoardLetters =
                [
                    new LetterOnBoard { Letter = Letter.C, Coordinate = new Coordinate(1, 1) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(1, 2) },
                    new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(1, 4) },
                ],
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(1, 3),
                Alignment = Alignment.Vertical,
                Letters = [Letter.S],
            }
        );

        Assert.True(valid);
    }

    [Fact]
    public async Task StackedPlacement()
    {
        var valid = await _sut.Validate(
            new Board
            {
                BoardLetters =
                [
                    new LetterOnBoard { Letter = Letter.W, Coordinate = new Coordinate(1, 1) },
                    new LetterOnBoard { Letter = Letter.H, Coordinate = new Coordinate(2, 1) },
                    new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(3, 1) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(4, 1) },
                ],
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(2, 2),
                Alignment = Alignment.Horizontal,
                Letters = [Letter.E, Letter.N, Letter.A, Letter.C, Letter.T],
            }
        );

        Assert.True(valid);
    }

    [Fact]
    public async Task StackedPlacement2()
    {
        var valid = await _sut.Validate(
            new Board
            {
                BoardLetters =
                [
                    new LetterOnBoard { Letter = Letter.P, Coordinate = new Coordinate(1, 1) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(2, 1) },
                    new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(3, 1) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(4, 1) },
                    new LetterOnBoard { Letter = Letter.M, Coordinate = new Coordinate(5, 1) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(6, 1) },
                    new LetterOnBoard { Letter = Letter.R, Coordinate = new Coordinate(7, 1) },
                    //
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(1, 2) },
                    new LetterOnBoard { Letter = Letter.M, Coordinate = new Coordinate(2, 2) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(3, 2) },
                    new LetterOnBoard { Letter = Letter.R, Coordinate = new Coordinate(4, 2) },
                    new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(5, 2) },
                    new LetterOnBoard { Letter = Letter.N, Coordinate = new Coordinate(6, 2) },
                    new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(7, 2) },
                    //
                    new LetterOnBoard { Letter = Letter.C, Coordinate = new Coordinate(1, 3) },
                    new LetterOnBoard { Letter = Letter.U, Coordinate = new Coordinate(2, 3) },
                    new LetterOnBoard { Letter = Letter.R, Coordinate = new Coordinate(3, 3) },
                    new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(4, 3) },
                    new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(5, 3) },
                    new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(6, 3) },
                    new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(7, 3) },
                    //
                    new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(1, 4) },
                    new LetterOnBoard { Letter = Letter.S, Coordinate = new Coordinate(2, 4) },
                    new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(3, 4) },
                    new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(4, 4) },
                    new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(5, 4) },
                    new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(6, 4) },
                    new LetterOnBoard { Letter = Letter.D, Coordinate = new Coordinate(7, 4) },
                ],
            },
            new PlacementModel
            {
                Coordinate = new Coordinate(1, 5),
                Alignment = Alignment.Horizontal,
                Letters = [Letter.R, Letter.E, Letter.S, Letter.E, Letter.E, Letter.D, Letter.S],
            }
        );

        Assert.True(valid);
    }
}
