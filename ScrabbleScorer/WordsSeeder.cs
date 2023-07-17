namespace ScrabbleScorer;

public class WordsSeeder
{
    public string[] SeedWords()
    {
        using var reader = new StreamReader("./Assets/words");

        var words = reader.ReadToEnd().Split(
            new[] { Environment.NewLine }, 
            StringSplitOptions.RemoveEmptyEntries);

        return words;
    }
}