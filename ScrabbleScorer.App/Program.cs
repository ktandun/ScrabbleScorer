// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using ScrabbleScorer.Core.Enums;
using ScrabbleScorer.Core.Logic.Rules;
using ScrabbleScorer.Core.Models;
using ScrabbleScorer.Core.Repositories;
using ScrabbleScorer.Core.Services;

var services = new ServiceCollection();
services.AddSingleton<IWordRepository, WordRepository>();
services.AddSingleton<IPlacementRule, PlacementShouldBeNextToExistingPlacements>();
services.AddSingleton<IPlacementRule, TileShouldBeEmpty>();
services.AddSingleton<IPlacementRule, WordsCreatedShouldBeValid>();
services.AddSingleton<IPlacementRule, WordShouldFitInsideBoard>();
services.AddSingleton<IGameService, GameService>();

var provider = services.BuildServiceProvider();

var gameService = provider.GetRequiredService<IGameService>();

await gameService.FindBestWordAsync(
    new Board
    {
        BoardLetters =
        [
            new LetterOnBoard { Letter = Letter.D, Coordinate = new Coordinate(5, 8) },
            new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(6, 8) },
            new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(7, 8) },
            new LetterOnBoard { Letter = Letter.X, Coordinate = new Coordinate(8, 8) },
            new LetterOnBoard { Letter = Letter.Y, Coordinate = new Coordinate(9, 8) },
            //
            new LetterOnBoard { Letter = Letter.K, Coordinate = new Coordinate(6, 6) },
            new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(6, 7) },
            new LetterOnBoard { Letter = Letter.S, Coordinate = new Coordinate(6, 9) },
            //
            new LetterOnBoard { Letter = Letter.J, Coordinate = new Coordinate(7, 5) },
            new LetterOnBoard { Letter = Letter.I, Coordinate = new Coordinate(7, 6) },
            new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(7, 7) },
            //
            new LetterOnBoard { Letter = Letter.N, Coordinate = new Coordinate(8, 4) },
            new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(8, 5) },
            new LetterOnBoard { Letter = Letter.R, Coordinate = new Coordinate(8, 6) },
            //
            new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(9, 4) },
            new LetterOnBoard { Letter = Letter.P, Coordinate = new Coordinate(10, 4) },
            new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(11, 4) },
            new LetterOnBoard { Letter = Letter.L, Coordinate = new Coordinate(12, 4) },
            new LetterOnBoard { Letter = Letter.I, Coordinate = new Coordinate(13, 4) },
            new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(14, 4) },
            new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(15, 4) },
            //
            new LetterOnBoard { Letter = Letter.D, Coordinate = new Coordinate(15, 1) },
            new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(15, 2) },
            new LetterOnBoard { Letter = Letter.F, Coordinate = new Coordinate(15, 3) },
            //
            new LetterOnBoard { Letter = Letter.Y, Coordinate = new Coordinate(11, 3) },
            new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(12, 3) },
            new LetterOnBoard { Letter = Letter.M, Coordinate = new Coordinate(13, 3) },
            //
            new LetterOnBoard { Letter = Letter.N, Coordinate = new Coordinate(9, 6) },
            new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(9, 7) },
            //
            new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(10, 5) },
            new LetterOnBoard { Letter = Letter.G, Coordinate = new Coordinate(11, 5) },
            new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(12, 5) },
            new LetterOnBoard { Letter = Letter.R, Coordinate = new Coordinate(13, 5) },
            //
            new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(12, 1) },
            new LetterOnBoard { Letter = Letter.V, Coordinate = new Coordinate(12, 2) },
            // new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(8, 8) },
            // new LetterOnBoard { Letter = Letter.T, Coordinate = new Coordinate(9, 8) },
            // new LetterOnBoard { Letter = Letter.O, Coordinate = new Coordinate(10, 8) },
            // new LetterOnBoard { Letter = Letter.N, Coordinate = new Coordinate(11, 8) },
            // new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(12, 8) },
            // //
            // new LetterOnBoard { Letter = Letter.H, Coordinate = new Coordinate(9, 6) },
            // new LetterOnBoard { Letter = Letter.A, Coordinate = new Coordinate(9, 7) },
            // new LetterOnBoard { Letter = Letter.E, Coordinate = new Coordinate(9, 9) },
        ],
    },
    new List<Letter> { Letter.T, Letter.Blank, Letter.B, Letter.E, Letter.O, Letter.O, Letter.I }
);
