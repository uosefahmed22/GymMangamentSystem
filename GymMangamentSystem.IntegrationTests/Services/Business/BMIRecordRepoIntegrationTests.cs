using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Enums.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Common;
using GymMangamentSystem.Reposatory.Data.Context;
using GymMangamentSystem.Reposatory.Services;
using GymMangamentSystem.Reposatory.Services.Business;
using Microsoft.EntityFrameworkCore;

namespace GymMangamentSystem.IntegrationTests.Services.Business;

public class BMIRecordRepoIntegrationTests
{
    [Fact]
    public async Task AddBMIRecord_WithValidData_PersistsRecordWithCalculatedCategory()
    {
        // Arrange
        var databaseName = $"GymMangamentSystem_IntegrationTests_{Guid.NewGuid():N}";
        const string expectedDatabasePrefix = "GymMangamentSystem_IntegrationTests_";

        if (!databaseName.StartsWith(expectedDatabasePrefix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Unsafe integration test database name.");
        }

        var connectionString =
            $"Server=(localdb)\\MSSQLLocalDB;Database={databaseName};" +
            "Trusted_Connection=True;TrustServerCertificate=True;" +
            "MultipleActiveResultSets=True";

        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlServer(connectionString)
            .Options;

        await using var dbContext = new AppDBContext(options);

        try
        {
            await dbContext.Database.EnsureCreatedAsync();

            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "integration-test-user",
                NormalizedUserName = "INTEGRATION-TEST-USER",
                Email = "integration-test@example.com",
                NormalizedEmail = "INTEGRATION-TEST@EXAMPLE.COM",
                DisplayName = "Integration Test User",
                UserCode = "INTEGRATION-TEST-USER-CODE",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var repository = new BMIRecordRepo(dbContext, new AppMapper());

            var dto = new BMIRecordDto
            {
                UserId = user.Id,
                WeightInKg = 80m,
                HeightInMeters = 2m
            };

            // Act
            var result = await repository.AddBMIRecord(dto);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Normal", result.Data);

            dbContext.ChangeTracker.Clear();

            var storedRecord = await dbContext.BMIRecords.SingleAsync();
            Assert.Equal(user.Id, storedRecord.UserId);
            Assert.Equal(80m, storedRecord.WeightInKg);
            Assert.Equal(2m, storedRecord.HeightInMeters);
            Assert.Equal(BMICategory.Normal, storedRecord.Category);
            Assert.False(storedRecord.IsDeleted);

            await repository.AddBMIRecord(new BMIRecordDto
            {
                UserId = user.Id,
                WeightInKg = 60m,
                HeightInMeters = 1.7m
            });
            await repository.AddBMIRecord(new BMIRecordDto
            {
                UserId = user.Id,
                WeightInKg = 90m,
                HeightInMeters = 1.8m
            });

            var secondPage = await repository.GetBMIRecordsForUser(
                user.Id,
                new PaginationParameters { PageNumber = 2, PageSize = 1 });

            Assert.Single(secondPage);
        }
        finally
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
