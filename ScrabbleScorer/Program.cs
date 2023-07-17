using ScrabbleScorer;
using ScrabbleScorer.Core.Utilities;
using ScrabbleScorer.Database;
using ScrabbleScorer.Database.Entities;
using ScrabbleScorer.Services;

var seeder = new WordsSeeder();
var words = seeder.SeedWords();

await using var database = new DatabaseContext();

if (!database.IsExist())
{
    database.Database.EnsureDeleted();
    database.Database.EnsureCreated();

    foreach (var word in words)
    {
        var wordEntity = new WordEntity
        {
            Word = word,
            WordSorted = word.ToLowerInvariant().ToSortedLetters(reverse: true)
        };

        database.Add(wordEntity);
    }

    await database.SaveChangesAsync();
}
