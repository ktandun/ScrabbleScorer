using Microsoft.EntityFrameworkCore;
using ScrabbleScorer.Database;
using ScrabbleScorer.ImageProcessing;

namespace ScrabbleScorer.Tests;

public class ImageUtilityTests
{
    [Test]
    public async Task ReadBoardImageTest()
    {
        await using var database = new DatabaseContext();

        var letterHashes = await database.LetterHashes
            .Select(l => new { Letter = l.Letter, Hash = ImageUtility.ImageHashFromString(l.Hash) })
            .ToArrayAsync();

        var foundLetters = new List<string>();
        
        var n = 15;
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                using var boardPart = ImageUtility.SplitBoardImage(
                    "/Users/kenzietandun/Downloads/test.png",
                    i,
                    j,
                    76
                );
                using var boardPartBlackAndWhite = ImageUtility.RemoveColorsFromImage(boardPart);
                using var boardPartCropped = ImageUtility.CropImage(boardPartBlackAndWhite, 8, 8, 42, 59);
                using var boardPartOnlyLetter = ImageUtility.CropOuterWhiteRegion(boardPartCropped);
                using var boardPartOnlyLetterPadded = ImageUtility.AddPaddingWithWhiteBackground(boardPartOnlyLetter, 64);
                
                var outputFilePath = $"output_{i + 1}_{j + 1}.png";

                ImageUtility.SaveImage(boardPartOnlyLetterPadded, outputFilePath);

                if (!ImageUtility.IsEmptyImage(boardPartOnlyLetter))
                {
                    var imageHash = ImageUtility.GenerateImageHash(outputFilePath);
                    
                    var matchingLetter = letterHashes.MaxBy(
                        lh =>
                            ImageUtility.CalculateImageHashSimilarity(
                                lh.Hash,
                                imageHash
                            )
                    );
                    
                    var similarities = letterHashes.Select(
                        lh => new
                        {
                            Letter = lh.Letter,
                            Similarity = ImageUtility.CalculateImageHashSimilarity(
                                lh.Hash,
                                imageHash
                            )
                        }
                    ).ToArray();
                
                    if (matchingLetter is not null)
                    {
                        foundLetters.Add($"{matchingLetter.Letter}_{i + 1}_{j + 1}");
                    }
                }
            }
        }

        var assertions = new[]
        {
            "b_1_8",
            "g_1_15",
            "u_2_8",
            "u_2_8",
            "i_2_15",
            "d_3_5",
            "i_3_6",
            "t_3_8",
            "a_3_9",
            "r_3_10",
            "y_3_11",
            "n_3_15",
            "j_4_4",
            "o_4_5",
            "t_4_8",
            "o_4_11",
            "f_4_12",
            "t_4_13",
            "e_4_14",
            "n_4_15",
            "l_5_2",
            "o_5_3",
            "a_5_4",
            "d_5_5",
            "s_5_6",
            "d_5_11",
            "e_5_12",
            "i_5_13",
            "t_5_14",
            "y_5_15",
            "s_6_1",
            "o_6_2",
            "u_6_3",
            "p_6_4",
            "q_6_6",
            "w_6_10",
            "e_6_11",
            "m_6_12",
            "o_7_1",
            "v_7_8",
            "l_7_11",
            "e_7_12",
            "i_7_13",
            "r_7_14",
            "s_7_15",
            "n_8_1",
            "l_8_3",
            "i_8_6",
            "e_8_8",
            "h_8_15",
            "a_9_1",
            "m_9_2",
            "e_9_3",
            "z_9_6",
            "a_9_7",
            "x_9_8",
            "f_9_13",
            "a_9_15",
            "t_10_1",
            "a_10_2",
            "r_10_3",
            "v_10_7",
            "i_10_13",
            "g_10_15",
            "a_11_1",
            "n_11_2",
            "e_11_3",
            "i_11_7",
            "b_11_10",
            "r_11_11",
            "i_11_12",
            "n_11_13",
            "e_11_14",
            "s_11_15",
            "e_12_2",
            "w_12_4",
            "a_12_5",
            "l_12_6",
            "d_12_7",
            "o_12_8",
            "c_12_13",
            "g_13_2",
            "u_13_8",
            "h_13_13",
            "o_13_14",
            "e_14_2",
            "r_14_8",
            "a_14_9",
            "k_14_10",
            "e_14_11",
            "n_15_8",
            "t_15_11",
            "r_15_12",
            "i_15_13",
            "c_15_14",
            "e_15_15",
        };
        
        Assert.Multiple(() =>
        {
            foreach (var assertion in assertions)
            {
                Assert.That(foundLetters, Does.Contain(assertion));
            }
        });
    }

    [Test]
    public void CropBoardSectionFromScreenshotTest()
    {
        using var boardSection = ImageUtility.CropBoardSectionFromScreenshot(
            "/Users/kenzietandun/Downloads/s.jpeg"
        );

        ImageUtility.SaveImage(boardSection, "/Users/kenzietandun/test.png");
    }
}
