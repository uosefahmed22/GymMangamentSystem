using GymMangamentSystem.Core.Enums.Business;
using GymMangamentSystem.Core.Models.Business;

namespace GymMangamentSystem.UnitTests.Models.Business;

public class BMIRecordExtensionsTests
{
    [Fact]
    public void CalculateBMI_WithValidWeightAndHeight_ReturnsExpectedBMI()
    {
        // Arrange
        var bmiRecord = new BMIRecord
        {
            WeightInKg = 80m,
            HeightInMeters = 2m
        };

        // Act
        var result = bmiRecord.CalculateBMI();

        // Assert
        Assert.Equal(20m, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateBMI_WithNonPositiveHeight_ThrowsArgumentException(decimal height)
    {
        // Arrange
        var bmiRecord = new BMIRecord
        {
            WeightInKg = 80m,
            HeightInMeters = height
        };

        // Act
        Action action = () => bmiRecord.CalculateBMI();

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(18.4, BMICategory.Underweight)]
    [InlineData(18.5, BMICategory.Normal)]
    [InlineData(24.9, BMICategory.Normal)]
    [InlineData(25.0, BMICategory.Overweight)]
    [InlineData(29.9, BMICategory.Overweight)]
    [InlineData(30.0, BMICategory.Obese)]
    [InlineData(34.9, BMICategory.Obese)]
    [InlineData(35.0, BMICategory.SeverelyObese)]
    public void DetermineBMICategory_AtCategoryBoundaries_ReturnsExpectedCategory(
        double bmi,
        BMICategory expectedCategory)
    {
        // Arrange
        var bmiAsDecimal = Convert.ToDecimal(bmi);

        // Act
        var result = bmiAsDecimal.DetermineBMICategory();

        // Assert
        Assert.Equal(expectedCategory, result);
    }
}
