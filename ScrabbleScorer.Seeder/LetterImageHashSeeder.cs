using Microsoft.EntityFrameworkCore;
using ScrabbleScorer.Database;
using ScrabbleScorer.Database.Entities;
using ScrabbleScorer.ImageProcessing;

namespace ScrabbleScorer;

public class LetterImageHashSeeder
{
    public async Task SeedLetterImageHashes()
    {
        await using var database = new DatabaseContext();

        await database.LetterHashes.ExecuteDeleteAsync();

        var alphabets = "abcdefghijklmnopqrstuvwxyz";

        foreach (var alphabet in alphabets)
        {
            var filename = $"./Assets/{alphabet}.png";

            var imageHash = ImageUtility.GenerateImageHash(
                ImageUtility.ReadImageFromFile(filename)
            );

            database.LetterHashes.Add(
                new LetterHashEntity
                {
                    Letter = alphabet,
                    Hash = ImageUtility.ImageHashToString(imageHash),
                }
            );
        }

        await database.SaveChangesAsync();
    }
}
