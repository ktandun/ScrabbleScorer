// See https://aka.ms/new-console-template for more information

using System.Text.Json;
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

var board = """
    {
      "BoardLetters": [
        { "Letter": 6, "Coordinate": { "X": 7, "Y": 8 } },
        { "Letter": 18, "Coordinate": { "X": 8, "Y": 8 } },
        { "Letter": 15, "Coordinate": { "X": 9, "Y": 8 } },
        { "Letter": 14, "Coordinate": { "X": 10, "Y": 8 } },
        { "Letter": 20, "Coordinate": { "X": 11, "Y": 8 } },
        { "Letter": 19, "Coordinate": { "X": 12, "Y": 8 } },
        { "Letter": 3, "Coordinate": { "X": 12, "Y": 9 } },
        { "Letter": 2, "Coordinate": { "X": 11, "Y": 10 } },
        { "Letter": 15, "Coordinate": { "X": 12, "Y": 10 } },
        { "Letter": 1, "Coordinate": { "X": 11, "Y": 11 } },
        { "Letter": 12, "Coordinate": { "X": 12, "Y": 11 } },
        { "Letter": 25, "Coordinate": { "X": 11, "Y": 12 } },
        { "Letter": 5, "Coordinate": { "X": 12, "Y": 12 } },
        { "Letter": 24, "Coordinate": { "X": 12, "Y": 13 } },
        { "Letter": 9, "Coordinate": { "X": 13, "Y": 13 } },
        { "Letter": 19, "Coordinate": { "X": 14, "Y": 13 } },
        { "Letter": 8, "Coordinate": { "X": 14, "Y": 14 } },
        { "Letter": 13, "Coordinate": { "X": 15, "Y": 14 } },
        { "Letter": 5, "Coordinate": { "X": 15, "Y": 15 } },
        { "Letter": 7, "Coordinate": { "X": 14, "Y": 8 } },
        { "Letter": 18, "Coordinate": { "X": 14, "Y": 9 } },
        { "Letter": 1, "Coordinate": { "X": 14, "Y": 10 } },
        { "Letter": 25, "Coordinate": { "X": 14, "Y": 11 } },
        { "Letter": 9, "Coordinate": { "X": 14, "Y": 12 } },
        { "Letter": 21, "Coordinate": { "X": 13, "Y": 9 } },
        { "Letter": 20, "Coordinate": { "X": 13, "Y": 10 } },
        { "Letter": 19, "Coordinate": { "X": 10, "Y": 11 } },
        { "Letter": 12, "Coordinate": { "X": 10, "Y": 12 } },
        { "Letter": 9, "Coordinate": { "X": 10, "Y": 13 } },
        { "Letter": 16, "Coordinate": { "X": 10, "Y": 14 } },
        { "Letter": 26, "Coordinate": { "X": 9, "Y": 7 } },
        { "Letter": 15, "Coordinate": { "X": 15, "Y": 8 } },
        { "Letter": 4, "Coordinate": { "X": 15, "Y": 9 } }
      ]
    }

    """;

await gameService.FindBestWord(
    JsonSerializer.Deserialize<Board>(board)!,
    [Letter.Q, Letter.U, Letter.O, Letter.T, Letter.E, Letter.D, Letter.J]
);
