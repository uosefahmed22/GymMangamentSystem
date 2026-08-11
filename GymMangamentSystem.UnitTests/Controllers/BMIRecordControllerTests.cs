using GymMangamentSystem.Core.Models.Common;
using System.Security.Claims;
using GymMangamentSystem.Apis.Controllers;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.UnitTests.Controllers;

public class BMIRecordControllerTests
{
    [Theory]
    [InlineData(200)]
    [InlineData(400)]
    [InlineData(500)]
    public async Task AddBMIRecord_WhenServiceReturnsStatus_ReturnsMatchingHttpStatus(
        int serviceStatusCode)
    {
        // Arrange
        var serviceResponse = new ApiResponse(serviceStatusCode, "Test response");
        var controller = CreateController(new StubBMIRecordRepo(addResponse: serviceResponse));
        var dto = new BMIRecordDto
        {
            WeightInKg = 70m,
            HeightInMeters = 1.75m
        };

        // Act
        var result = await controller.AddBMIRecord(dto);

        // Assert
        var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal(serviceStatusCode, objectResult.StatusCode);
        Assert.Same(serviceResponse, objectResult.Value);
    }

    [Theory]
    [InlineData(200)]
    [InlineData(404)]
    [InlineData(500)]
    public async Task DeleteBMIRecord_WhenServiceReturnsStatus_ReturnsMatchingHttpStatus(
        int serviceStatusCode)
    {
        // Arrange
        var serviceResponse = new ApiResponse(serviceStatusCode, "Test response");
        var controller = CreateController(new StubBMIRecordRepo(deleteResponse: serviceResponse));

        // Act
        var result = await controller.DeleteBMIRecord(1);

        // Assert
        var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal(serviceStatusCode, objectResult.StatusCode);
        Assert.Same(serviceResponse, objectResult.Value);
    }

    private static BMIRecordController CreateController(IBMIRecordRepo repository)
    {
        var identity = new ClaimsIdentity(
            new[] { new Claim("UserId", "test-user-id") },
            "TestAuthentication");

        return new BMIRecordController(repository)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            }
        };
    }

    private sealed class StubBMIRecordRepo(
        ApiResponse? addResponse = null,
        ApiResponse? deleteResponse = null) : IBMIRecordRepo
    {
        public Task<ApiResponse> AddBMIRecord(BMIRecordDto bmiRecord)
        {
            return Task.FromResult(addResponse ?? throw new NotSupportedException());
        }

        public Task<ApiResponse> DeleteBMIRecord(int id)
        {
            return Task.FromResult(deleteResponse ?? throw new NotSupportedException());
        }

        public Task<IEnumerable<object>> GetBMIRecordsForUser(string userId, PaginationParameters? pagination = null)
        {
            throw new NotSupportedException();
        }
    }
}
