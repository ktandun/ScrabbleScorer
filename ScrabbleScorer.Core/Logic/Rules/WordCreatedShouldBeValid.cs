namespace ScrabbleScorer.Core.Logic.Rules;

public class WordCreatedShouldBeValid : IPlacementRule
{
    public bool Validate(Board board, PlacementModel placement)
    {
        var wordCreated = new List<Letter>();
        var currentCoordinate = placement.Coordinate;

        for (var i = 0; i < placement.Letters.Count; i++)
        {
            var letter = board.GetLetterInCoordinate(currentCoordinate);

            if (letter is not null)
            {
                wordCreated.Add(letter.Value);
                i--;
            }
            else
            {
                wordCreated.Add(placement.Letters[i]);
            }

            currentCoordinate = currentCoordinate.NextTile(placement.Alignment);
        }

        var word = new string(wordCreated.Select(l => l.ToChar()).ToArray());
        var validWords = new List<string> { "valid" }; // todo: get dictionary words

        return validWords.Contains(word);
    }
}
