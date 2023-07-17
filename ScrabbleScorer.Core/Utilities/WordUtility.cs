namespace ScrabbleScorer.Core.Utilities;

public static class WordUtility
{
    public static Dictionary<char, int> CountLettersInWord(string word)
    {
        var dictionary = new Dictionary<char, int>();

        foreach (var letter in word)
        {
            if (dictionary.ContainsKey(letter))
            {
                dictionary[letter] += 1;
            }
            else
            {
                dictionary.Add(letter, 1);
            }
        }

        return dictionary;
    }

    public static string ToSortedLetters(this string word, bool reverse = false)
    {
        return reverse
            ? new string(word.OrderByDescending(c => c).ToArray())
            : new string(word.OrderBy(c => c).ToArray());
    }

    public static string[] DifferentPermutationsOfBlank(this string word, int numBlanks)
    {
        if (numBlanks == 0)
        {
            return new[] { word };
        }

        return Enumerable
            .Range(0, word.Length + 1)
            .Select(e => word.Insert(e, "_"))
            .SelectMany(w => w.DifferentPermutationsOfBlank(numBlanks - 1))
            .ToArray();
    }

    public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(
        this IEnumerable<T> elements,
        int k
    )
    {
        var enumerable = elements as T[] ?? elements.ToArray();

        return k == 0
            ? new[] { Array.Empty<T>() }
            : enumerable.SelectMany(
                (e, i) =>
                    enumerable.Skip(i + 1).DifferentCombinations(k - 1).Select(c => c.Append(e))
            );
    }
}
