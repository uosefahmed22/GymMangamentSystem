using GymMangamentSystem.Core.Models.Common;

namespace GymMangamentSystem.UnitTests.Models.Common;

public class PaginationParametersTests
{
    [Fact]
    public void Defaults_AreFirstPageWithTwentyItems()
    {
        var pagination = new PaginationParameters();

        Assert.Equal(1, pagination.PageNumber);
        Assert.Equal(20, pagination.PageSize);
        Assert.Equal(0, pagination.Skip);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-10, 1)]
    [InlineData(150, 100)]
    public void PageSize_IsClampedToSafeRange(int requested, int expected)
    {
        var pagination = new PaginationParameters { PageSize = requested };

        Assert.Equal(expected, pagination.PageSize);
    }

    [Fact]
    public void Skip_UsesNormalizedPageNumber()
    {
        var pagination = new PaginationParameters { PageNumber = 3, PageSize = 10 };

        Assert.Equal(20, pagination.Skip);
    }
}
