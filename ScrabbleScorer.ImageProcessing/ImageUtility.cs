using SkiaSharp;

namespace ScrabbleScorer.ImageProcessing;

public static class ImageUtility
{
    public static SKBitmap SplitBoardImage(string path, int i, int j, int imageSize)
    {
        var originalImage = SKBitmap.Decode(path);

        var regionWidth = imageSize;
        var regionHeight = imageSize;

        var region = new SKBitmap(regionWidth, regionHeight);

        var sourceRect = new SKRectI(
            i * regionWidth,
            j * regionHeight,
            (i + 1) * regionWidth,
            (j + 1) * regionHeight
        );
        originalImage.ExtractSubset(region, sourceRect);

        return region;
    }

    public static SKBitmap RemoveColorsFromImage(SKBitmap originalImage)
    {
        const byte threshold = 128;
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

        var newImageHeight =
            Math.Abs(boardEndHeightPixel!.Value - boardStartHeightPixel!.Value) + 1;
        var outputImage = new SKBitmap(image.Width, newImageHeight);

        using var canvas = new SKCanvas(outputImage);

        var sourceRect = new SKRect(
            18,
            boardEndHeightPixel.Value + 18,
            image.Width,
            boardStartHeightPixel.Value
        );
        canvas.DrawBitmap(image, sourceRect, SKRect.Create(image.Width, newImageHeight));

        return outputImage;
    }

    public static SKBitmap CropImage(SKBitmap originalImage, int top, int left, int height, int width)
    {
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
        for (var x = 0; x < skBitmap.Width; x++)
        {
            for (var y = 0; y < skBitmap.Height; y++)
            {
                var color = skBitmap.GetPixel(x, y);

                if (!IsWhiteish(color))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool[] GenerateImageHash(string filePath)
    {
        using var image = SKBitmap.Decode(filePath);

        var hash = new List<bool>();

        for (var x = 0; x < image.Width; x++)
        {
            for (var y = 0; y < image.Height; y++)
            {
                var color = image.GetPixel(x, y);

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
