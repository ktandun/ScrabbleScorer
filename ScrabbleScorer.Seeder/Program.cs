using ScrabbleScorer;
using ScrabbleScorer.Core.Utilities;
using ScrabbleScorer.Database;
using ScrabbleScorer.Database.Entities;

var seeder = new WordsSeeder();
var words = seeder.SeedWords();

await using var database = new DatabaseContext();

// database.Database.EnsureDeleted();
// database.Database.EnsureCreated();
//
// foreach (var word in words)
// {
//     var wordEntity = new WordEntity
//     {
//         Word = word,
//         WordSorted = word.ToLowerInvariant().ToSortedLetters(reverse: true)
//     };
//
//     database.Add(wordEntity);
// }
//
// await database.SaveChangesAsync();

var letterHashSeeder = new LetterImageHashSeeder();
await letterHashSeeder.SeedLetterImageHashes();
