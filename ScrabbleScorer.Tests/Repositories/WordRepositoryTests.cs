using Microsoft.Extensions.Caching.Memory;
using ScrabbleScorer.Core.Repositories;
using Xunit;

namespace ScrabbleScorer.Tests.Repositories;

public class WordRepositoryTests
{
    private readonly WordRepository _sut;

    public WordRepositoryTests()
    {
        _sut = new WordRepository(new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public async Task ReadWordsAsyncTest()
    {
        var result = await _sut.ReadWordsAsync();

        Assert.NotNull(result);
    }
}
