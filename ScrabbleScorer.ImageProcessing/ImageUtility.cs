using SkiaSharp;

namespace ScrabbleScorer.ImageProcessing;

public static class ImageUtility
{
    private const int BoardHeight = 1170;
    private const int BoardWidth = 1170;

    public static SKBitmap ConvertToMonochromeWithContrast(SKBitmap bitmap, float contrastFactor)
    {
        var outputBitmap = new SKBitmap(bitmap.Width, bitmap.Height);

        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var pixelColor = bitmap.GetPixel(x, y);
                var grayValue = (byte)(
                    0.2989 * pixelColor.Red + 0.5870 * pixelColor.Green + 0.1140 * pixelColor.Blue
                );

                var newPixelColor = new SKColor(grayValue, grayValue, grayValue, pixelColor.Alpha);
                outputBitmap.SetPixel(x, y, newPixelColor);
            }
        }

        return outputBitmap;
    }

    public static SKBitmap ReadImageFromFile(string filename)
    {
        return SKBitmap.Decode(filename);
    }

    public static SKBitmap SplitBoardImage(SKBitmap bitmap, int i, int j, int imageSize)
    {
        var regionWidth = imageSize;
        var regionHeight = imageSize;

        var region = new SKBitmap(regionWidth, regionHeight);

        var sourceRect = new SKRectI(
            i * regionWidth,
            j * regionHeight,
            (i + 1) * regionWidth,
            (j + 1) * regionHeight
        );
        bitmap.ExtractSubset(region, sourceRect);

        return region;
    }

    public static SKBitmap RemoveColorsFromImage(SKBitmap originalImage)
    {
        const byte threshold = 180;
        var bwImage = new SKBitmap(originalImage.Width, originalImage.Height);

        for (var x = 0; x < originalImage.Width; x++)
        {
            for (var y = 0; y < originalImage.Height; y++)
            {
                var originalColor = originalImage.GetPixel(x, y);

                var grayValue = (byte)(
                    (originalColor.Red + originalColor.Green + originalColor.Blue) / 3
                );

                var bwColor = grayValue >= threshold ? SKColors.White : SKColors.Black;

                bwImage.SetPixel(x, y, bwColor);
            }
        }

        return bwImage;
    }

    public static SKBitmap CropBoardSectionFromScreenshot(string filePath)
    {
        using var image = SKBitmap.Decode(filePath);

        int? boardStartHeightPixel = null;
        int? boardEndHeightPixel = null;

        foreach (var height in Enumerable.Range(1, image.Height).Reverse())
        {
            if (IsWhiteish(image.GetPixel(1, height)) && boardStartHeightPixel is null)
            {
                boardStartHeightPixel = height;
            }
            else if (boardStartHeightPixel is not null && !IsWhiteish(image.GetPixel(1, height)))
            {
                boardEndHeightPixel = height;
                break;
            }
        }

        var outputImage = new SKBitmap(BoardWidth, BoardHeight);

        using var canvas = new SKCanvas(outputImage);

        var sourceRect = new SKRect(
            17,
            boardEndHeightPixel.Value + 16,
            BoardWidth,
            boardEndHeightPixel.Value + BoardHeight
        );

        canvas.DrawBitmap(image, sourceRect, SKRect.Create(BoardWidth, BoardHeight));

        return outputImage;
    }

    public static SKBitmap CropImage(
        SKBitmap originalImage,
        int top,
        int left,
        int height,
        int width
    )
    {
        var outputImage = new SKBitmap(width, height);

        using var canvas = new SKCanvas(outputImage);

        var sourceRect = new SKRect(left, top, width + left, height + top);
        canvas.DrawBitmap(originalImage, sourceRect, SKRect.Create(width, height));

        return outputImage;
    }

    public static SKBitmap CropLetterFromImage(SKBitmap originalImage)
    {
        var (startX, startY) = (0, 0);
        var outputImage = new SKBitmap(width, height);

        using var canvas = new SKCanvas(outputImage);

        var sourceRect = new SKRect(left, top, width + left, height + top);
        canvas.DrawBitmap(originalImage, sourceRect, SKRect.Create(width, height));

        return outputImage;
    }

    public static void SaveImage(SKBitmap skBitmap, string outputFilePath)
    {
        using var image = SKImage.FromBitmap(skBitmap);
        using var encoded = image.Encode();
        using Stream stream = File.OpenWrite(outputFilePath);

        encoded.SaveTo(stream);
    }

    private static bool IsWhiteish(SKColor skColor)
    {
        return skColor is { Red: > 240, Blue: > 240, Green: > 240 };
    }

    public static bool IsEmptyImage(SKBitmap skBitmap)
    {
        var whiteCount = 0;
        var blackCount = 0;

        for (var x = 0; x < skBitmap.Width; x++)
        {
            for (var y = 0; y < skBitmap.Height; y++)
            {
                var color = skBitmap.GetPixel(x, y);

                whiteCount += IsWhiteish(color) ? 1 : 0;
                blackCount += IsWhiteish(color) ? 0 : 1;
            }
        }

        return whiteCount > blackCount && (double)whiteCount / (whiteCount + blackCount) > 0.9;
    }

    public static bool IsDarkImage(SKBitmap skBitmap)
    {
        var whiteCount = 0;
        var blackCount = 0;

        for (var x = 0; x < skBitmap.Width; x++)
        {
            for (var y = 0; y < skBitmap.Height; y++)
            {
                var color = skBitmap.GetPixel(x, y);

                whiteCount += IsWhiteish(color) ? 1 : 0;
                blackCount += IsWhiteish(color) ? 0 : 1;
            }
        }

        return blackCount > whiteCount && (double)blackCount / (whiteCount + blackCount) > 0.9;
    }

    public static bool[] GenerateImageHash(SKBitmap bitmap)
    {
        var hash = new List<bool>();

        for (var x = 0; x < bitmap.Width; x++)
        {
            for (var y = 0; y < bitmap.Height; y++)
            {
                var color = bitmap.GetPixel(x, y);

                hash.Add(color == SKColors.White);
            }
        }

        return hash.ToArray();
    }

    public static string ImageHashToString(bool[] imageHash)
    {
        return string.Join("", imageHash.Select(ih => ih ? "1" : "0"));
    }

    public static bool[] ImageHashFromString(string hashString)
    {
        return hashString.Select(hs => hs == '1').ToArray();
    }

    public static float CalculateImageHashSimilarity(bool[] hashOne, bool[] hashTwo)
    {
        var similarCount = hashOne.Where((t, i) => t == hashTwo[i]).Count();

        return (float)similarCount / hashOne.Length;
    }

    public static SKBitmap CropOuterWhiteRegion(SKBitmap inputImage)
    {
        var width = inputImage.Width;
        var height = inputImage.Height;

        var top = -1;
        var bottom = -1;
        var left = -1;
        var right = -1;

        var foundNonWhitePixel = false;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var pixelColor = inputImage.GetPixel(x, y);

                if (pixelColor.Red < 255 || pixelColor.Green < 255 || pixelColor.Blue < 255)
                {
                    if (!foundNonWhitePixel)
                    {
                        top = y;
                        foundNonWhitePixel = true;
                    }
                    bottom = y;
                    left = (left == -1 || x < left) ? x : left;
                    right = (right == -1 || x > right) ? x : right;
                }
            }
        }

        if (!foundNonWhitePixel)
        {
            return inputImage;
        }

        var cropWidth = right - left + 1;
        var cropHeight = bottom - top + 1;

        var croppedImage = new SKBitmap(cropWidth, cropHeight);

        for (var y = 0; y < cropHeight; y++)
        {
            for (var x = 0; x < cropWidth; x++)
            {
                croppedImage.SetPixel(x, y, inputImage.GetPixel(left + x, top + y));
            }
        }

        return croppedImage;
    }

    public static SKBitmap AddPaddingWithWhiteBackground(SKBitmap inputImage, int size)
    {
        var originalWidth = inputImage.Width;
        var originalHeight = inputImage.Height;

        var paddingWidth = Math.Max(0, size - originalWidth);
        var paddingHeight = Math.Max(0, size - originalHeight);

        var paddedImage = new SKBitmap(size, size);

        using var canvas = new SKCanvas(paddedImage);
        canvas.Clear(SKColors.White);

        var x = paddingWidth / 2;
        var y = paddingHeight / 2;

        canvas.DrawBitmap(inputImage, SKRect.Create(x, y, originalWidth, originalHeight));

        return paddedImage;
    }
}
