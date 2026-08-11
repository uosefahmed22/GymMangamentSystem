using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Data.Context;
using GymMangamentSystem.Reposatory.Services;
using GymMangamentSystem.Reposatory.Services.Business;
using Microsoft.EntityFrameworkCore;

namespace GymMangamentSystem.UnitTests.Services.Business;

public class BMIRecordRepoTests
{
    [Theory]
    [InlineData(0, 1.75)]
    [InlineData(-70, 1.75)]
    [InlineData(70, 0)]
    [InlineData(70, -1.75)]
    public async Task AddBMIRecord_WithNonPositiveWeightOrHeight_ReturnsBadRequest(
        double weight,
        double height)
    {
        // Arrange
        var dbContextOptions = new DbContextOptionsBuilder<AppDBContext>().Options;
        await using var dbContext = new AppDBContext(dbContextOptions);
        var repository = new BMIRecordRepo(dbContext, new AppMapper());

        var dto = new BMIRecordDto
        {
            WeightInKg = Convert.ToDecimal(weight),
            HeightInMeters = Convert.ToDecimal(height)
        };

        // Act
        var result = await repository.AddBMIRecord(dto);

        // Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Weight and height must be greater than 0.", result.Message);
    }
}
