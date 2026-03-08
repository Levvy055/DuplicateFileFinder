using DuplicateFileFinder;
using Xunit;

namespace DuplicateFileFinder.Tests.Unit;

public class ResultsTests
{
    [Fact]
    public void Results_NewInstance_SameNamesCollectionIsInitialized()
    {
        var results = new Results();

        Assert.NotNull(results.SameNames);
        Assert.Empty(results.SameNames);
    }

    [Fact]
    public void Results_NewInstance_SameSizesCollectionIsInitialized()
    {
        var results = new Results();

        Assert.NotNull(results.SameSizes);
        Assert.Empty(results.SameSizes);
    }
}
