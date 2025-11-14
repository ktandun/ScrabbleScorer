using ScrabbleScorer.Core.Repositories;

namespace ScrabbleScorer.Tests.Repositories;

public class WordRepositoryTests
{
    private readonly WordRepository _sut;

    public WordRepositoryTests()
    {
        _sut = new WordRepository();
    }

    [Fact]
    public async Task ReadWordsAsyncTest()
    {
        var result = await _sut.GetDictionaryWordsAsync();

        Assert.NotNull(result);
    }
}
